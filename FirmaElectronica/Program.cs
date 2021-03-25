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
            string pathArchivo = "C:\\FacturacionElectronica\\Docs_Generados\\FA1903202101099034750600120010010000235839999999911.xml";//RUTA DEL ARCHIVO XML INCLUIDO NOMBRE
            string pathFirma = "C:\\GroupTechnology\\blanca_nelly_ramon_ortega.p12"; //RUTA DE LA FIRMA ELECTRONICA INCLUIDO NOMBRE DEL ARCHIVO
            string claveFirma = ""; //CLAVE DE LA FIRMA ELECTRONICA
            string pathArchivoFirmado = "C:\\FacturacionElectronica\\Docs_Firmados"; //RUTA DONDE SE ALMACENARA EL ARCHIVO FIRMADO
            string nombreArchivoSalida = "FA1903202101099034750600120010010000235839999999911.xml";

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
