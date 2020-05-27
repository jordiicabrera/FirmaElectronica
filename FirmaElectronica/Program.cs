using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaElectronica
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathArchivo = "";//RUTA DEL ARCHIVO XML
            string pathFirma = ""; //RUTA DE LA FIRMA ELECTRONICA  
            string claveFirma = ""; //CLAVE DE LA FIRMA ELECTRONICA
            string pathArchivoFirmado = ""; //RUTA DONDE SE ALMACENARA EL ARCHIVO FIRMADO
            string nombreArchivoSalida = "";

            string mensaje = "";

            try
            {
                //INSTANCIAMOS LA CLASE PARA SER USADA
                FirmarXML firma = new FirmarXML();
                if (firma.Firmar(pathFirma, claveFirma, pathArchivo, pathArchivoFirmado+@"\"+nombreArchivoSalida,ref mensaje)){
                    Console.WriteLine("Archivo firmado con exito");
                }
                else
                {
                    Console.WriteLine(mensaje);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
