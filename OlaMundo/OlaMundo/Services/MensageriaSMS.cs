using System;
using System.Collections.Generic;
using System.Text;

namespace OlaMundo.Services
{
    internal class MensageriaSMS
    {
        public bool EnviarMensagem(string texto)
        {
            Console.WriteLine("Enviando SMS: " + texto);
            return true;
        }
    }
}
