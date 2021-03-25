using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Runtime.InteropServices;
using java.security.cert;
using java.security;
using es.mityc.javasign.trust;
using java.io;
using es.mityc.firmaJava.libreria.xades;
using es.mityc.javasign.xml.refs;
using org.w3c.dom;
using es.mityc.firmaJava.libreria.utilidades;
using javax.xml.parsers;
using java.util;
using sviudes.blogspot.com;
using es.mityc.javasign.pkstore.keystore;
using es.mityc.javasign.pkstore;
using Console = System.Console;
using es.mityc.javasign.xml.xades.policy;
using System.IO;

namespace FirmaElectronica
{
    public class FirmarXML
    {
        public Boolean Firmar(string RutaCertificado, string Clave, string RutaXML, string RutaFirmado, ref string mensaje)
        {
            try
            {
                X509Certificate certificate = default(X509Certificate);
                PrivateKey key = default(PrivateKey);
                Provider provider = default(Provider);
                string str = Clave;
                KeyStore store = KeyStore.getInstance("PKCS12");
                store.load(new FileInputStream(RutaCertificado), str.ToCharArray());
                Enumeration enumeration = store.aliases();
                while (enumeration.hasMoreElements())
                {
                    string alias1 = Convert.ToString(enumeration.nextElement());
                    if (store.isKeyEntry(alias1))
                    {
                        //certificate = (X509Certificate)store.getCertificate(alias1);
                        certificate = (X509Certificate)store.getCertificate(alias1);
                        key = (PrivateKey)store.getKey(alias1, str.ToCharArray());
                        provider = store.getProvider();
                        break;
                    }
                }
                PrivateKey key2 = key;
                Provider provider2 = provider;
                if (certificate != null)
                {
                    TrustFactory.instance = TrustFactory.newInstance();
                    TrustFactory.truster = PropsTruster.getInstance();
                    DataToSign dataToSign = new DataToSign();
                    dataToSign.setXadesFormat(EnumFormatoFirma.XAdES_BES);
                    dataToSign.setEsquema(XAdESSchemas.XAdES_132);
                    dataToSign.setPolicyKey("facturae31");
                    dataToSign.setXMLEncoding("UTF-8");
                    dataToSign.setEnveloped(true);
                    dataToSign.addObject(new ObjectToSign(new InternObjectToSign("comprobante"), "contenido comprobante", null, "text/xml", null));
                    dataToSign.setParentSignNode("comprobante");
                    dataToSign.setDocument(LoadXML(RutaXML));
                    object[] objArray = new FirmaXML().signFile(certificate, dataToSign, key, provider);
                    FileOutputStream outputStream = new FileOutputStream(RutaFirmado);
                    UtilidadTratarNodo.saveDocumentToOutputStream((Document)objArray[0], outputStream, true);
                    outputStream.flush();
                    outputStream.close();
                }
                return true;
            }
            catch (Exception exception1)
            {
                //ProjectData.SetProjectError(exception1);
                //Exception exception = exception1;
                mensaje = "Error al Firmar el Documento : " + exception1.Message;
                //ProjectData.ClearProjectError();
                return false;
                //ProjectData.ClearProjectError();
            }
        }

        private Document LoadXML(string path)
        {
            DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
            factory.setNamespaceAware(true);
            return factory.newDocumentBuilder().parse(new BufferedInputStream(new FileInputStream(path)));
        }


        private static X509Certificate LoadCertificate(string RutaCertificado, string Clave, out PrivateKey privateKey, out Provider provider)
        {
            X509Certificate certificate = null;
            provider = null;
            privateKey = null;

            //Cargar certificado de fichero PFX  
            KeyStore ks = KeyStore.getInstance("PKCS12");
            ks.load(new FileInputStream(RutaCertificado), Clave.ToCharArray());
            IPKStoreManager storeManager = new KSStore(ks, new PassStoreKS(""));
            List certificates = storeManager.getSignCertificates();

            //Si encontramos el certificado...  
            if (certificates.size() >= 1)
            {
                certificate = (X509Certificate)certificates.get(0);

                // Obtención de la clave privada asociada al certificado  
                privateKey = storeManager.getPrivateKey(certificate);

                // Obtención del provider encargado de las labores criptográficas  
                provider = storeManager.getProvider(certificate);
            }

            return certificate;
        }

        public bool SignXml(string RutaCertificado, string Clave, string RutaXML, string RutaFirmado, ref string mensaje)
        {


            //if (!ValidateAccessKey)
            //    return false;

            PrivateKey privateKey;
            Provider provider;

            try
            {

                //X509Certificate certificate = LoadCertificate("","", out privateKey, out provider);

                X509Certificate certificate = null;
                provider = null;
                privateKey = null;

                //Cargar certificado de fichero PFX  
                KeyStore ks = KeyStore.getInstance("PKCS12");
                ks.load(new FileInputStream(RutaCertificado), Clave.ToCharArray());
                IPKStoreManager storeManager = new KSStore(ks, new PassStoreKS(Clave));
                List certificates = storeManager.getSignCertificates();

                //Si encontramos el certificado...  
                if (certificates.size() >= 1)
                {
                    certificate = (X509Certificate)certificates.get(0);

                    // Obtención de la clave privada asociada al certificado  
                    privateKey = storeManager.getPrivateKey(certificate);

                    // Obtención del provider encargado de las labores criptográficas  
                    provider = storeManager.getProvider(certificate);
                }

                if (certificate != null)
                {
                    TrustFactory.instance = es.mityc.javasign.trust.TrustExtendFactory.newInstance();
                    TrustFactory.truster = es.mityc.javasign.trust.MyPropsTruster.getInstance();
                    PoliciesManager.POLICY_SIGN = new es.mityc.javasign.xml.xades.policy.facturae.Facturae31Manager();

                    com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl s = new com.sun.org.apache.xerces.@internal.jaxp.SAXParserFactoryImpl();

                    PoliciesManager.POLICY_VALIDATION = new es.mityc.javasign.xml.xades.policy.facturae.Facturae31Manager();

                    DataToSign dataToSign = new DataToSign();
                    dataToSign.setXadesFormat(EnumFormatoFirma.XAdES_BES); //XAdES-EPES
                    dataToSign.setEsquema(XAdESSchemas.XAdES_132);
                    dataToSign.setPolicyKey("facturae31");
                    //dataToSign.setAddPolicy(true);
                    dataToSign.setAddPolicy(false);
                    dataToSign.setXMLEncoding("UTF-8");
                    dataToSign.setEnveloped(true);
                    dataToSign.addObject(new ObjectToSign(new InternObjectToSign("comprobante"), "contenido comprobante", null, "text/xml", null));

                    //string fileToSign = Path.Combine("", fileName);

                    Document doc = LoadXML(RutaXML);
                    dataToSign.setDocument(doc);

                    //dataToSign.setDocument(IDocumentoElectronicoExtensions.LoadXml(fileToSign));

                    Object[] res = new FirmaXML().signFile(certificate, dataToSign, privateKey, provider);

                    java.io.FileOutputStream file = new FileOutputStream(RutaFirmado);

                    UtilidadTratarNodo.saveDocumentToOutputStream((Document)res[0], file, true);
                    file.flush();
                    file.close();

                    //DeleteFile(fileToSign);
                }

                return true;
            }
            catch (Exception ex)
            {

                //System.Windows.Forms.MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message);

                //System.Diagnostics.EventLog.WriteEntry("BcLog", "SignXml - Error en Certificado " + ex.Message);
                return false;
            }
        }


    }
}
