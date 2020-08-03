using BackEndAsisApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;


namespace BackEndAsisApp
{
    /// <summary>
    /// Summary description for webservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class webservice : System.Web.Services.WebService
    {

        //Modelo de seguridad
        ApplicationDbContext context = new ApplicationDbContext();

        //Modelo DB
        DBAsisApp db = new DBAsisApp();

        //El registro inicia con el usuario DIRECTOR
        #region Registro

        public class ResultRegister
        {
            public bool Band { get; set; }
            public string Mensaje { get; set; }
        }

        public string Pass { get; private set; }
        [WebMethod]
        public ResultRegister Register(string email, string pass, string nombre, string apellido)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ResultRegister Value = new ResultRegister();

            Director Existe = db.Directors.Where(x => x.Email == email).FirstOrDefault();

            if (Existe == null)
            {
                var user = new ApplicationUser();
                user.UserName = email;
                user.Email = email;



                var chkUser = UserManager.Create(user, pass);


                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Director");

                    Director director = new Director();
                    director.Nombre = nombre;
                    director.Apellido = apellido;
                    director.Email = email;
                    db.Directors.Add(director);
                    db.SaveChanges();
                    Value.Band = true;
                    Value.Mensaje = "Se creo el usuario con exito";

                }
                else
                {
                    Value.Band = false;
                    Value.Mensaje = "Ocurrio un error no se pudo crear";
                }
            }
            else
            {
                Value.Band = false;
                Value.Mensaje = "Ya existe un usuario con esa cuenta";
            }

            return Value;
        }



        #endregion

        #region Logueo

        //Este metodo es privado

        //Este es un metodo interno NO Publico
       public bool Validar(string AccountName, string Pass)
        {

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            // var  result = SignInManager.PasswordSignInAsync(user, pass, false, shouldLockout: false);
            var result = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>().PasswordSignIn(AccountName, Pass, false, false);


            if (result == SignInStatus.Success)
            {
                return true;
            }
            else
                return false;
        }

        //WebMethod de logeo
        [WebMethod]
        public bool Login(string email, string pass)
        {
            return Validar(email, pass);
        }

        #endregion

        #region Departamento

        //Agregar 
        [WebMethod]
        public bool AddDepartamento(string nomdepartamento)
        {
            var newDepartamento = new Departamento { NomDepartamento = nomdepartamento };
            db.Departamentoes.Add(newDepartamento);
            return db.SaveChanges() > 0;

        }

        //Eliminar
        [WebMethod]
        public bool EliminarDepartamento(int id)
        {
            var deleteDepartamento = db.Departamentoes.FirstOrDefault(x => x.IdDepartamento == id);
            db.Departamentoes.Remove(deleteDepartamento);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class DepartamentoSW
        {
            public int Id { get; set; }
            public string NomDepartamento { get; set; }
        }


        //Listado
        [WebMethod]
        public List<DepartamentoSW> ListaDepartamento()
        {
            return db.Departamentoes.Select(x => new DepartamentoSW()
            {
                Id = x.IdDepartamento,
                NomDepartamento = x.NomDepartamento
            }).ToList();
        }

        #endregion

        #region AnioElectivo

        //Agregar 
        [WebMethod]
        public bool AddAnioElectivo(string descripcion, string semestre)
        {
            var newAnioElectivo = new AnioElectivo { Descripcion = descripcion, Semestre = semestre };
            db.AnioElectivoes.Add(newAnioElectivo);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarAnioElectivo(int id)
        {
            var deleteAnioElectivo = db.AnioElectivoes.FirstOrDefault(x => x.IdAnioElectivo == id);
            db.AnioElectivoes.Remove(deleteAnioElectivo);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class AnioElectivoSW
        {
            public int _Id { get; set; }
            public string _Descripcion { get; set; }
            public string _Semestre { get; set; }
        }

        //Buscar
        [WebMethod]
        public List<AnioElectivoSW> BuscarAnioElectivo(int id)
        {

            return db.AnioElectivoes.Where(x => x.IdAnioElectivo == id).Select(x => new AnioElectivoSW()
            {
                _Id = x.IdAnioElectivo,
                _Descripcion = x.Descripcion,
                _Semestre = x.Semestre
            }).ToList();

        }

        //Listado
        [WebMethod]
        public List<AnioElectivoSW> ListaAnioElectivo()
        {
            return db.AnioElectivoes.Select(x => new AnioElectivoSW()
            {
                _Id = x.IdAnioElectivo,
                _Descripcion = x.Descripcion,
                _Semestre = x.Semestre
            }).ToList();
        }

        #endregion

        #region Colegio

        //Agregar
        [WebMethod]
        public bool AddColegio(string nomcolegio, int iddepartamento, int idanioelectivo, int iddirector)
        {
            var newColegio = new Colegio { NomColegio = nomcolegio, IdDepartamento = iddepartamento, IdAnioElectivo = idanioelectivo, IdDirector = iddirector };
            db.Colegios.Add(newColegio);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarColegio(int id)
        {
            var deleteColegio = db.Colegios.FirstOrDefault(x => x.idColegio == id);
            db.Colegios.Remove(deleteColegio);
            return db.SaveChanges() > 0;
        }

        public class ColegioInnerJoin
        {
            public int _Id { get; set; }
            public string _NomColegio { get; set; }
            public string _NomDepartamento { get; set; }
            public string _NomAnioElectivo { get; set; }
        }

        //Listado
        [WebMethod]
        public List<ColegioInnerJoin> ListaColegioInner()
        {

            var query = from cole in db.Colegios
                        join depar in db.Departamentoes on cole.IdDepartamento equals depar.IdDepartamento
                        join anio in db.AnioElectivoes on cole.IdAnioElectivo equals anio.IdAnioElectivo
                        select new ColegioInnerJoin
                        {
                            _Id = cole.idColegio,
                            _NomColegio = cole.NomColegio,
                            _NomDepartamento = depar.NomDepartamento,
                            _NomAnioElectivo = anio.Descripcion
                        };

            return query.ToList();
        }

        //Serializacion para listar y buscar
        public class ColegioSW
        {
            public int _Id { get; set; }
            public string _NomColegio { get; set; }
            public int _IdDepartamento { get; set; }
            public int _IdAnioElectivo { get; set; }
        }

        //Listado
        [WebMethod]
        public List<ColegioSW> ListaSpinnerCole()
        {
            return db.Colegios.Select(x => new ColegioSW()
            {
                _Id = x.idColegio,
                _NomColegio = x.NomColegio

            }).ToList();
        }

        #endregion

        #region Grado

        //Agregar
        [WebMethod]
        public bool AddGrado(string descripcion, int idcolegio)
        {
            var newGrado = new Grado { Descripcion = descripcion, idColegio = idcolegio };
            db.Gradoes.Add(newGrado);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarGrado(int id)
        {
            var deleteGrado = db.Gradoes.FirstOrDefault(x => x.IdGrado == id);
            db.Gradoes.Remove(deleteGrado);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class GradoSW
        {
            public int _Id { get; set; }
            public string _Descripcion { get; set; }
            public int _idColegio { get; set; }

        }

        //Listado
        [WebMethod]
        public List<GradoSW> ListaSpinnerGrado()
        {
            return db.Gradoes.Select(x => new GradoSW()
            {
                _Id = x.IdGrado,
                _Descripcion = x.Descripcion

            }).ToList();
        }

        public class GradoInnerJoin
        {
            public int _Id { get; set; }
            public string _NomGrado { get; set; }
            public string _NomColegio { get; set; }

        }
        //Listado
        [WebMethod]
        public List<GradoInnerJoin> ListaGradoInner()
        {

            var query = from gra in db.Gradoes
                        join colee in db.Colegios on gra.idColegio equals colee.idColegio
                        select new GradoInnerJoin
                        {
                            _Id = gra.idColegio,
                            _NomGrado = gra.Descripcion,
                            _NomColegio = colee.NomColegio

                        };

            return query.ToList();
        }

        #endregion

        #region Materia

        //Agregar
        [WebMethod]
        public bool AddMateria(string nommateria)
        {
            var newMateria = new Materia { NomMateria = nommateria };
            db.Materias.Add(newMateria);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarMateria(int id)
        {
            var deleteMateria = db.Materias.FirstOrDefault(x => x.IdMateria == id);
            db.Materias.Remove(deleteMateria);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class MateriaSW
        {
            public int _Id { get; set; }
            public string _NomMateria { get; set; }

        }


        //Listado
        [WebMethod]
        public List<MateriaSW> ListaSpinnerMateria()
        {
            return db.Materias.Select(x => new MateriaSW()
            {
                _Id = x.IdMateria,
                _NomMateria = x.NomMateria

            }).ToList();
        }

        //Listado
        //[WebMethod]
        //public List<MateriaInnerJoin> ListaMateriaInner()
        //{

        //    var query = from materia in db.Materias
        //                join alum in db.Alumnoes on materia.IdAlumno equals alum.IdAlumno
        //                select new MateriaInnerJoin
        //                {
        //                    _Id = materia.IdMateria,
        //                    _NomMateria = materia.NomMateria,
        //                    _Nombre = alum.Nombre,
        //                    _Apellido = alum.Apellido,

        //                };

        //    return query.ToList();
        //}

        #endregion

        #region Alumno

        //Agregar
        [WebMethod]
        public bool AddAlumno(string nombre, string apellido, string sexo, string telefono, string email, int idgrado, int idgrupo, int idtutor)
        {
            var newAlumno = new Alumno { Nombre = nombre, Apellido = apellido, Sexo = sexo, Telefono = telefono, Email = email, IdGrado = idgrado, IdGrupo = idgrupo, IdTutor = idtutor };
            db.Alumnoes.Add(newAlumno);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarAlumno(int id)
        {
            var deleteAlumno = db.Alumnoes.FirstOrDefault(x => x.IdAlumno == id);
            db.Alumnoes.Remove(deleteAlumno);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class AlumnoSW
        {
            public int _Id { get; set; }
            public string _Nombre { get; set; }
            public string _Apellido { get; set; }
            public string _Sexo { get; set; }
            public string _Telefono { get; set; }
            public string _Email { get; set; }
            public int _IdGrado { get; set; }

        }

        [WebMethod]
        public List<AlumnoSW> ListaSpinnerAlumno()
        {
            return db.Alumnoes.Select(x => new AlumnoSW()
            {
                _Id = x.IdAlumno,
                _Nombre = x.Nombre + " " + x.Apellido

            }).ToList();
        }

        public class AlumnoInnerJoin
        {
            public int _Id { get; set; }
            public string _NomCompleto { get; set; }
            public string _Sexo { get; set; }
            public string _Telefono { get; set; }
            public string _Email { get; set; }
            public string _Grado { get; set; }

        }
        //Listado
        [WebMethod]
        public List<AlumnoInnerJoin> ListaAlumnoInner()
        {

            var query = from alum in db.Alumnoes
                        join gra in db.Gradoes on alum.IdGrado equals gra.IdGrado
                        select new AlumnoInnerJoin
                        {
                            _Id = alum.IdAlumno,
                            _NomCompleto = alum.Nombre + " " + alum.Apellido,
                            _Sexo = alum.Sexo,
                            _Telefono = alum.Telefono,
                            _Email = alum.Email,
                            _Grado = gra.Descripcion

                        };

            return query.ToList();
        }
        //Listado


        #endregion

        #region EnumAsis

        //Agregar 
        [WebMethod]
        public bool AddEnumAsis(string nomEnum)
        {
            var newEnum = new EmunAsi { Descripcion = nomEnum };
            db.EmunAsis.Add(newEnum);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarEnumAsis(int id)
        {
            var deleteDepartamento = db.EmunAsis.FirstOrDefault(x => x.IdEnumAsis == id);
            db.EmunAsis.Remove(deleteDepartamento);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class EnumAsisSW
        {
            public int Id { get; set; }
            public string EnumAsis { get; set; }
        }



        //Listado
        [WebMethod]
        public List<EnumAsisSW> ListaEnum()
        {
            return db.EmunAsis.Select(x => new EnumAsisSW()
            {
                Id = x.IdEnumAsis,
                EnumAsis = x.Descripcion
            }).ToList();
        }

        #endregion

        #region Asistencia

        //Agregar
        [WebMethod]
        public bool AddAsistencia(DateTime fecha, int idalumno, int idenumasis, int iddocente)
        {
            var newAsistencia = new Asistencia { Fecha = fecha, IdAlumno = idalumno, IdEnumAsis = idenumasis, IdDocente = iddocente };
            db.Asistencias.Add(newAsistencia);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarAsistencia(int id)
        {
            var deleteAsistencia = db.Asistencias.FirstOrDefault(x => x.IdAsistencia == id);
            db.Asistencias.Remove(deleteAsistencia);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class AsistenciaSW
        {
            public int _Id { get; set; }
            public DateTime? _Fecha { get; set; }
            public int _IdAlumno { get; set; }
            public int _IdEnumAsis { get; set; }



        }

        //Listado

        public class AsistenciaInnerJoin
        {
            public int _Id { get; set; }
            public string _Fecha { get; set; }
            public string _NomAlumno { get; set; }
            public string _NomAsistencia { get; set; }
            public string _NomMateria { get; set; }
            public string _NomGrado { get; set; }
            public string _NomGrupo { get; set; }
            public string _TipoAsistencia { get; set; }
        }

        //Listado
        //[WebMethod]
        //public List<AsistenciaInnerJoin> ListaAsistenciaInner()
        //{

        //    var query = from asis in db.Asistencias
        //                join alum in db.Alumnoes on asis.IdAlumno equals alum.IdAlumno
        //                join enumas in db.EmunAsis on asis.IdEnumAsis equals enumas.IdEnumAsis
        //                select new AsistenciaInnerJoin
        //                {
        //                    _Id = asis.IdAsistencia,
        //                    _Fecha = asis.Fecha.ToString(),
        //                    _NomAlumno = alum.Nombre + " " + alum.Apellido,
        //                    _NomAsistencia = enumas.Descripcion
        //                };

        //    return query.ToList();
        //}

        [WebMethod]
        public List<AsistenciaInnerJoin> ListaAsistenciaInner()
        {

            var query = from alumno in db.Alumnoes
                        join grupo in db.Grupoes on alumno.IdGrupo equals grupo.IdGrupo
                        join grado in db.Gradoes on alumno.IdGrado equals grado.IdGrado
                        join materia in db.Materias on grupo.IdMateria equals materia.IdMateria
                        join asistencia in db.Asistencias on alumno.IdAlumno equals asistencia.IdAlumno
                        join enumu in db.EmunAsis on asistencia.IdEnumAsis equals enumu.IdEnumAsis
                        select new AsistenciaInnerJoin
                        {
                            _Id = asistencia.IdAsistencia,
                            _Fecha = asistencia.Fecha.ToString(),
                            _NomAlumno = alumno.Nombre + " " + alumno.Apellido,
                            _NomMateria = materia.NomMateria,
                            _NomGrado = grado.Descripcion,
                            _NomGrupo = grupo.NomGrupo,
                            _NomAsistencia = enumu.Descripcion
                        };

            return query.ToList();
        }
        #endregion

        //-- Nuevo

        #region Grupo
        //Agregar
        [WebMethod]
        public bool AddGrupo(string descripcion, int iddocente, int idmateria)
        {
            var newGrupo = new Grupo { NomGrupo = descripcion, IdDocente = iddocente, IdMateria = idmateria };
            db.Grupoes.Add(newGrupo);
            return db.SaveChanges() > 0;
        }

        //Eliminar
        [WebMethod]
        public bool EliminarGrupo(int id)
        {
            var deleteGrupo = db.Grupoes.FirstOrDefault(x => x.IdGrupo == id);
            db.Grupoes.Remove(deleteGrupo);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class GrupoSW
        {
            public int _Id { get; set; }
            public string _Nomgrupo { get; set; }
            public int _idMateria { get; set; }
            public int _idDocente { get; set; }

        }

        //Listado
        [WebMethod]
        public List<GrupoSW> ListaSpinneGrupo()
        {
            return db.Grupoes.Select(x => new GrupoSW()
            {
                _Id = x.IdGrupo,
                _Nomgrupo = x.NomGrupo,
                _idMateria = x.IdMateria,
                _idDocente = x.IdDocente

            }).ToList();
        }

        public class GrupoInnerJoin
        {
            public int _Id { get; set; }
            public string _NomMateria { get; set; }
            public string _NomDocente { get; set; }
            public string _NomGrupo { get; set; }

        }
        //Listado
        [WebMethod]
        public List<GrupoInnerJoin> ListaGrupoInner()
        {

            var query = from grup in db.Grupoes
                        join mate in db.Materias on grup.IdMateria equals mate.IdMateria
                        join doc in db.Docentes on grup.IdDocente equals doc.IdDocente
                        select new GrupoInnerJoin
                        {
                            _Id = grup.IdGrupo,
                            _NomGrupo = grup.NomGrupo,
                            _NomMateria = mate.NomMateria,
                            _NomDocente = doc.Nombre + " " + doc.Apellido

                        };

            return query.ToList();
        }

        #endregion

        #region Tutor User

        //Agregar Tutor
        [WebMethod]
        public bool AddDTutor(string Nombre, string Apellido, string Telefono, string email)
        {
            Tutor tutor = new Tutor();
            tutor.Nombre = Nombre;
            tutor.Apellido = Apellido;
            tutor.Telefono = Telefono;
            tutor.Email = email;

            db.Tutors.Add(tutor);

            if (db.SaveChanges() > 0)
            {
                //Si se guardo la informacion del docente
                //Agregamos un usuario para ese docente
                var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = new ApplicationUser();
                //Asignamos los vslores
                user.UserName = email;
                user.Email = email;
                string pwd = "123456";
                //procedemos a agregar el usuario
                var verificar = ManejadorUsuario.Create(user, pwd);

                //Asignamos el usuario con su respectivo rol

                if (verificar.Succeeded)
                {
                    var result = ManejadorUsuario.AddToRole(user.Id, "Tutor");
                    return true;
                }

                return true;
            }
            else
            { return false; }
        }



        //Eliminar
        [WebMethod]
        public bool EliminarTutor(int id)
        {
            var deleteTutor = db.Tutors.FirstOrDefault(x => x.IdTutor == id);
            db.Tutors.Remove(deleteTutor);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class TutorSW
        {
            public int _Id { get; set; }
            public string _NombreCompletoTutor { get; set; }
            public string _Email { get; set; }
            public string _Telefono { get; set; }

        }

        //Listado
        [WebMethod]
        public List<TutorSW> ListaTutor()
        {
            return db.Tutors.Select(x => new TutorSW()
            {
                _Id = x.IdTutor,
                _NombreCompletoTutor = x.Nombre + " " + x.Apellido,
                _Email = x.Email,
                _Telefono = x.Telefono
            }).ToList();
        }

        #endregion

        #region Docente User

        //Agregar Docente
        [WebMethod]
        public bool AddDocente(string Nombre, string Apellido, string Telefono, byte[] foto, string email)
        {
            Docente doc = new Docente();
            doc.Nombre = Nombre;
            doc.Apellido = Apellido;
            doc.Telefono = Telefono;
            doc.Foto = foto;
            doc.Email = email;

            db.Docentes.Add(doc);

            if (db.SaveChanges() > 0)
            {
                //Si se guardo la informacion del docente
                //Agregamos un usuario para ese docente
                var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = new ApplicationUser();
                //Asignamos los vslores
                user.UserName = email;
                user.Email = email;
                string pwd = "123456";
                //procedemos a agregar el usuario
                var verificar = ManejadorUsuario.Create(user, pwd);

                //Asignamos el usuario con su respectivo rol

                if (verificar.Succeeded)
                {
                    var result = ManejadorUsuario.AddToRole(user.Id, "Docente");
                    return true;
                }

                return true;
            }
            else
            { return false; }


        }

        //Eliminar
        [WebMethod]
        public bool EliminarDocente(int id)
        {
            var deleteDocente = db.Docentes.FirstOrDefault(x => x.IdDocente == id);
            db.Docentes.Remove(deleteDocente);
            return db.SaveChanges() > 0;

        }

        //Serializacion para listar y buscar
        public class DocenteSW
        {
            public int _Id { get; set; }
            public string _Nombre { get; set; }
            public string _Apellido { get; set; }
            public string _Email { get; set; }
            public string _Telefono { get; set; }
            public byte[] _Foto { get; set; }

        }


        //Listado
        [WebMethod]
        public List<DocenteSW> ListaDocente()
        {
            return db.Docentes.Select(x => new DocenteSW()
            {
                _Id = x.IdDocente,
                _Nombre = x.Nombre + " " + x.Apellido,
                _Email = x.Email,
                _Telefono = x.Telefono,
                _Foto = x.Foto
                

            }).ToList();
        }

        #endregion

        //El admin podra crear usuarios Directores
        #region Director User

        //Agregar Director
        [WebMethod]
        public bool AddDirector(string Nombre, string Apellido, string email)
        {
            Director director = new Director();
            director.Nombre = Nombre;
            director.Apellido = email;
            director.Email = email;

            db.Directors.Add(director);

            if (db.SaveChanges() > 0)
            {
                //Si se guardo la informacion del docente
                //Agregamos un usuario para ese docente
                var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = new ApplicationUser();
                //Asignamos los vslores
                user.UserName = email;
                user.Email = email;
                string pwd = "123456";
                //procedemos a agregar el usuario
                var verificar = ManejadorUsuario.Create(user, pwd);

                //Asignamos el usuario con su respectivo rol

                if (verificar.Succeeded)
                {
                    var result = ManejadorUsuario.AddToRole(user.Id, "Director");
                    return true;
                }

                return true;
            }
            else
            { return false; }
        }





        //Serializacion para listar y buscar
        public class DirectorSW
        {
            public int _Id { get; set; }
            public string _Nombre { get; set; }
            public string _Apellido { get; set; }
            public string _Email { get; set; }

        }


        //Listado
        [WebMethod]
        public List<DirectorSW> ListaDirector()
        {
            return db.Directors.Select(x => new DirectorSW()
            {
                _Id = x.IdDirector,
                _Nombre = x.Nombre + " " + x.Apellido, 
                _Email = x.Email
            }).ToList();
        }


        //Eliminar
        [WebMethod]
        public bool EliminarDirector(int id)
        {
            var deleteDirector = db.Directors.FirstOrDefault(x => x.IdDirector == id);
            db.Directors.Remove(deleteDirector);
            return db.SaveChanges() > 0;

        }

        #endregion

        


    }
}
