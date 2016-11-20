using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;

namespace PdfRpt.Core.Security
{
    /// <summary>
    /// Applies PDF Encryption Options
    /// </summary>
    public class EncryptionWorker
    {
        #region Properties (3)

        /// <summary>
        /// Sets the encryption options for this document.
        /// Leave it as null if you don't want to use it.
        /// </summary>
        public DocumentSecurity DocumentSecurity { get; set; }

        /// <summary>
        /// Document object
        /// </summary>
        public Document PdfDoc { get; set; }

        /// <summary>
        /// PdfWriter Object
        /// </summary>
        public PdfWriter PdfWriter { get; set; }

        #endregion Properties

        #region Methods (6)

        // Public Methods (2)

        /// <summary>
        /// Enable Encryption
        /// </summary>
        public void ApplyEncryption()
        {
            //encryption can only be added before opening the document
            if (DocumentSecurity == null) return;
            if (DocumentSecurity.EncryptionPreferences == null) return;

            var permissionsList = new List<int>();
            var permissions = 0;

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions == null)
                throw new InvalidOperationException("Please set the DocumentPermissions.");

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowPrinting)
            {
                permissions |= PdfWriter.AllowPrinting;
                permissionsList.Add(PdfWriter.AllowPrinting);
            }

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowModifyContents)
            {
                permissions |= PdfWriter.AllowModifyContents;
                permissionsList.Add(PdfWriter.AllowModifyContents);
            }

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowCopy)
            {
                permissions |= PdfWriter.AllowCopy;
                permissionsList.Add(PdfWriter.AllowCopy);
            }

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowModifyAnnotations)
            {
                permissions |= PdfWriter.AllowModifyAnnotations;
                permissionsList.Add(PdfWriter.AllowModifyAnnotations);
            }

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowFillIn)
            {
                permissions |= PdfWriter.AllowFillIn;
                permissionsList.Add(PdfWriter.AllowFillIn);
            }

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowScreenReaders)
            {
                permissions |= PdfWriter.AllowScreenReaders;
                permissionsList.Add(PdfWriter.AllowScreenReaders);
            }

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowAssembly)
            {
                permissions |= PdfWriter.AllowAssembly;
                permissionsList.Add(PdfWriter.AllowAssembly);
            }

            if (DocumentSecurity.EncryptionPreferences.DocumentPermissions.AllowDegradedPrinting)
            {
                permissions |= PdfWriter.AllowDegradedPrinting;
                permissionsList.Add(PdfWriter.AllowDegradedPrinting);
            }

            setPasswordEncryption(permissions);
            setPublicKeyEncryption(permissionsList);
        }

        /// <summary>
        /// Applies a digital signature to a document
        /// </summary>
        public void ApplySignature(Stream pdfStreamOutput)
        {
            if (DocumentSecurity == null) return;
            if (DocumentSecurity.DigitalSignature == null) return;
            if (DocumentSecurity.EncryptionPreferences == null) return;

            // close the document without closing the underlying stream
            PdfWriter.CloseStream = false;
            PdfDoc.Close();
            pdfStreamOutput.Position = 0;

            signPasswordEncryption(pdfStreamOutput);
            signPublicKeyEncryption(pdfStreamOutput);
        }
        // Private Methods (4)

        private void setPasswordEncryption(int permissions)
        {
            if (DocumentSecurity.EncryptionPreferences.EncryptionType != EncryptionType.PasswordEncryption) return;
            if (string.IsNullOrEmpty(DocumentSecurity.EncryptionPreferences.PasswordEncryption.EditPassword)) return;

            byte[] readPassword = null;
            if (!string.IsNullOrEmpty(DocumentSecurity.EncryptionPreferences.PasswordEncryption.ReadPassword))
            {
                readPassword = Encoding.UTF8.GetBytes(DocumentSecurity.EncryptionPreferences.PasswordEncryption.ReadPassword);
            }
            var editPassword = Encoding.UTF8.GetBytes(DocumentSecurity.EncryptionPreferences.PasswordEncryption.EditPassword);
            PdfWriter.SetEncryption(readPassword, editPassword, permissions, PdfWriter.STRENGTH128BITS);
        }

        private void setPublicKeyEncryption(List<int> permissionsList)
        {
            if (DocumentSecurity.EncryptionPreferences.EncryptionType != EncryptionType.PublicKeyEncryption) return;

            if (permissionsList.Count == 0) permissionsList.Add(PdfWriter.AllowScreenReaders);
            var certs = PfxReader.ReadCertificate(DocumentSecurity.EncryptionPreferences.PublicKeyEncryption.PfxPath, DocumentSecurity.EncryptionPreferences.PublicKeyEncryption.PfxPassword);
            PdfWriter.SetEncryption(
                      certs: certs.X509PrivateKeys,
                      permissions: permissionsList.ToArray(),
                      encryptionType: PdfWriter.ENCRYPTION_AES_128);

        }

        private void signPasswordEncryption(Stream pdfStreamOutput)
        {
            if (DocumentSecurity.EncryptionPreferences.EncryptionType != EncryptionType.PasswordEncryption) return;
            if (string.IsNullOrEmpty(DocumentSecurity.EncryptionPreferences.PasswordEncryption.EditPassword)) return;

            var editPassword = Encoding.UTF8.GetBytes(DocumentSecurity.EncryptionPreferences.PasswordEncryption.EditPassword);
            DocumentSecurity.DigitalSignature.CertificateFile.AppendSignature = true;

            new SignatureWriter
            {
                SignatureData = new Signature
                {
                    CertificateFile = DocumentSecurity.DigitalSignature.CertificateFile,
                    SigningInfo = DocumentSecurity.DigitalSignature.SigningInfo,
                    VisibleSignature = DocumentSecurity.DigitalSignature.VisibleSignature
                }
            }.SignPdf(pdfStreamOutput, editPassword);
        }

        private void signPublicKeyEncryption(Stream pdfStreamOutput)
        {
            if (DocumentSecurity.EncryptionPreferences.EncryptionType != EncryptionType.PublicKeyEncryption) return;

            var pfxData = PfxReader.ReadCertificate(DocumentSecurity.EncryptionPreferences.PublicKeyEncryption.PfxPath, DocumentSecurity.EncryptionPreferences.PublicKeyEncryption.PfxPassword);
            DocumentSecurity.DigitalSignature.CertificateFile.AppendSignature = true;

            new SignatureWriter
            {
                SignatureData = new Signature
                {
                    CertificateFile = DocumentSecurity.DigitalSignature.CertificateFile,
                    SigningInfo = DocumentSecurity.DigitalSignature.SigningInfo,
                    VisibleSignature = DocumentSecurity.DigitalSignature.VisibleSignature
                }
            }.SignPdf(pdfStreamOutput, pfxData);
        }

        #endregion Methods
    }
}
