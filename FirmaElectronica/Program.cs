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
            string pathArchivo = "C:\\FacturacionElectronica\\Docs_Generados\\FA2403202101093076400600110010010000000371234567812.xml";//RUTA DEL ARCHIVO XML INCLUIDO NOMBRE
            string pathFirma = "C:\\GroupTechnology\\archivo.p12"; //RUTA DE LA FIRMA ELECTRONICA INCLUIDO NOMBRE DEL ARCHIVO
            string claveFirma = ""; //CLAVE DE LA FIRMA ELECTRONICA
            string pathArchivoFirmado = "C:\\FacturacionElectronica\\Docs_Firmados"; //RUTA DONDE SE ALMACENARA EL ARCHIVO FIRMADO
            string nombreArchivoSalida = "FA2403202101093076400600110010010000000371234567812.xml";

            string mensaje = "";

            try
            {
                //INSTANCIAMOS LA CLASE PARA SER USADA
                FirmarXML firma = new FirmarXML();
                //FIRMADO ANTIGUO SOLO PRODUCCION
                //if (firma.Firmar(pathFirma, claveFirma, pathArchivo, pathArchivoFirmado+@"\"+nombreArchivoSalida,ref mensaje)){
                //    Console.WriteLine("Archivo firmado con exito");
                //}
                //else
                //{
                //    Console.WriteLine(mensaje);
                //}

                //NUEVO FIRMADO PRUEBAS Y PRODUCCION
                if (firma.SignXml(pathFirma, claveFirma, pathArchivo, pathArchivoFirmado + @"\" + nombreArchivoSalida, ref mensaje))
                {
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
