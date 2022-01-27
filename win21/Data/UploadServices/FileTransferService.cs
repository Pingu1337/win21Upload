using System;
using System.IO;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace win21.Data.UploadServices
{
    public class FileTransferService
    {
        private readonly ILogger<FileTransferService> _logger;
        private ftpClient _ftpClient;
        public FileTransferService(ILogger<FileTransferService> logger, ftpClient ftpClient)
        {
            _logger = logger;
            _ftpClient = ftpClient;
        }

        public async Task recursiveDirectory(string dirPath, string userid, string repo)
        {

            var ftpClient = _ftpClient;

            string uploadPath = $"{userid}/{repo}/";

            ftpClient.CreateFTPDirectory(userid);

            ftpClient.CreateFTPDirectory(uploadPath);

            string[] files = Directory.GetFiles(dirPath, "*.*");
            string[] subDirs = Directory.GetDirectories(dirPath);

            try
            {
                foreach (string file in files)
                {
                    await ftpClient.upload(uploadPath + Path.GetFileName(file), file);
                }

                foreach (string subDir in subDirs)
                { 
                    ftpClient.CreateFTPDirectory(uploadPath + Path.GetFileName(subDir));
                    recursiveDirectory(subDir, uploadPath, Path.GetFileName(subDir));
                }

            }
            catch (Exception e)
            {
                _logger.LogError("ftpClient Error: {e}", e);
            }
        }

    }

    public class ftpClient
    {

        private readonly IConfiguration _config;

        public ftpClient(IConfiguration config)
        {
            _config = config;
            
        }

        public async Task upload(string uploadPath, string fileName)
        {
            var Credentials = _config.GetSection("Credentials");

            var host = Credentials["Host"];
            var ftpUsername = Credentials["Username"];
            var ftpPassword = Credentials["Password"];

            if (host !=null && ftpUsername != null && ftpPassword != null)
            {

            }
            else
            {

            }

            using (var client = new WebClient())
            {
                
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                client.UploadFile(host + uploadPath, WebRequestMethods.Ftp.UploadFile, fileName);

            }

        }

        public bool CreateFTPDirectory(string directory)
        {
            try
            {
                var Credentials = _config.GetSection("Credentials");

                var host = Credentials["Host"];
                var ftpUsername = Credentials["Username"];
                var ftpPassword = Credentials["Password"];

                if (host != null && ftpUsername != null && ftpPassword != null)
                {

                }
                else
                {

                }


                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(host + directory);
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();

                response.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }
    }
}
