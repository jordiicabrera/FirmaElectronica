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
                catch (IOException exception1)
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

        }
    }
