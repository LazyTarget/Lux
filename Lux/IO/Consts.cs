using System.Text;

namespace Lux.IO
{
    public class Consts
    {
        public static readonly Encoding DefaultEncoding                 = Encoding.UTF8;


        public class ErrorMessages
        {
            public const string ERROR_DIR_NOT_FOUND                     = "Directory not found";
            public const string ERROR_FILE_NOT_FOUND                    = "Could not find file at specified path";
            public const string ERROR_SOURCE_FILE_NOT_FOUND             = "Source file was not found";
            public const string ERROR_SOURCE_DIR_NOT_FOUND              = "Source directory was not found";
            public const string ERROR_TARGET_FILE_ALREADY_EXISTS        = "Target file already exists, overwrite is disabled";
            public const string ERROR_TARGET_DIR_NOT_FOUND              = "Target directory was not found";
            public const string ERROR_TARGET_DIR_ALREADY_EXISTS         = "Target directory already exists";
            public const string ERROR_TARGET_SAME_AS_SOURCE             = "Target path cannot be the same as the source path";
            public const string ERROR_CREATING_DIR                      = "Error creating directory";
            public const string ERROR_DIR_NOT_EMPTY                     = "Directory is not empty";

        }

    }
}
