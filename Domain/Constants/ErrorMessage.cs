using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class ErrorMessage
    {
        public const string UserId = "Debe ingresar Id de usuario";
        public const string Names = "Debe ingresar nombre";
        public const string Surnames = "Debe ingresar Apellido";
        public const string Email = "Debe ingresar un correo valido";
        public const int MaxLengthPassword = 18;
        public const int MinLengthPassword = 6;
        public const string Title = "Debe ingresar titulo";
        public const string Keyword = "Debe ingresar palabras claves";
        public const string Content = "Debe ingresar contenido";

        public const string EmailOrPassword = "Debe ingresar un correo/contraseña valido";
        public const string UserExists = "El correo ya existe.";
        public const string UserBlocked = "El usuario se encuentra bloquedao, contacte al Administrador.";
        public const string UserNotLogin = "El correo o la contraseña no es valida.";
        public const string UserPending = "El correo todavia no fue verificado, revise su correo en la carpeta de spam.";
        public const string ResetPassword = "Debe cambiar la contraseña";
        public const string InfoInvalid = "La informacion es invalida";


        public const int IERR_OK = 0;
        public const string SERR_OK = "Funcionamiento Correcto";
       
        public const int IERR_NO_LOCAL_DATA = -1;
        public const string SERR_NO_LOCAL_DATA = "No hay datos locales";

        public const int IERR_IDENTITY_NOT_FOUND = -2;
        public const string SERR_IDENTITY_NOT_FOUND = "Identidad no encontrada";

        public const int IERR_IDENTITY_EXIST = -3;
        public const string SERR_IDENTITY_EXIST = "Ya existe la identidad";

        public const int IERR_DATA_INVALID = -4;
        public const string SERR_DATA_INVALID = "Dato invalido.";

        public const int IERR_CREATE_REGISTER = -5;
        public const string SERR_CREATE_REGISTER = "No se pudo crear el registro.";

        public const int IERR_UPDATE_REGISTER = -6;
        public const string SERR_UPDATE_REGISTER = "No se pudo actualizar el registro.";

        public const int IERR_WITHOUT_REGISTER = -7;
        public const string SERR_WITHOUT_REGISTER = "No hay registro.";

        public const int IERR_DELETE_REGISTER = -8;
        public const string SERR_DELETE_REGISTER = "No se pudo eliminar el registro.";

        public const int IERR_EXCEPTION = -9;
        public const string SERR_EXCEPTION = "Excepcion: ";
    }
}
