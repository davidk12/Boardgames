using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Boardgames_webproject.Models
{

    /// <summary>
    /// A singleton pattern error logger
    /// </summary>
    public class ErrorLogger
    {
        BoardGameRepository repository = BoardGameRepository.getRepository;

        private static ErrorLogger logger;

        private ErrorLogger() { }

        public static ErrorLogger getLogger
        {
            get
            {
                if (logger == null)
                {
                    logger = new ErrorLogger();
                }
                return logger;
            }
        }

        public void logIntoDatabase(Exception type, string function)
        {
            repository.logException(type.GetType().ToString(), type.Message, function);
        }
    }
}