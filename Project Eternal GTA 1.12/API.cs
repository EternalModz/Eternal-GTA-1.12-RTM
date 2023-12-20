using DevExpress.XtraEditors;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace authguard
{
    public class API
    {
        //API-Class v2.0

        public string program_version, program_key, api_enc_key, session_iv;
        private bool is_initialized, show_messages, logged_in;
        public bool logged = false;
        public API(string version, string program_key, string api_enc_key, bool show_messages = true)
        {
            this.program_version = version;

            this.program_key = program_key;

            this.api_enc_key = api_enc_key;

            this.show_messages = show_messages;
        }
        public void init()
        {
            try
            {
                session_iv = encryption.iv_key();
                var init_iv = encryption.sha256(session_iv);

                var values_to_upload = new NameValueCollection
                {
                    ["version"] = encryption.encrypt(program_version, api_enc_key, init_iv),
                    ["program_key"] = encryption.saltStr(program_key),
                    ["timestamp"] = encryption.saltStr(DateTime.Now.ToString()),
                    ["init_iv"] = encryption.saltStr(init_iv)
                };

                var response = do_request("init", values_to_upload);
                errCheck(response);
                security.End();
                response = encryption.decrypt(response, api_enc_key, init_iv);

                var decoded_response = response_decoder.string_to_generic<response_structure>(response);
               
                if (!decoded_response.success)
                    XtraMessageBox.Show(decoded_response.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                var response_data = decoded_response.response.Split('|');

                switch (response_data[0])
                {
                    case "started_program":
                        if (response_data[3] == "0" && response_data[4].Length > 0)
                        {
                            if (response_data[4] != security.Signature(Process.GetCurrentProcess().MainModule.FileName))
                            {
                                XtraMessageBox.Show("File has been tampered with, couldn't verify integrity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Process.GetCurrentProcess().Kill();
                            }
                        }
                        else appSettings.devmode = true;

                        if (response_data[5] == "1")
                        {
                            appSettings.freemode = true;
                        }

                        is_initialized = true;
                        session_iv += response_data[2];
                        break;

                    case "update_available":
                        if (response_data[2].Length > 0) Process.Start(response_data[2]);
                        else XtraMessageBox.Show("Please add a valid download link!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Process.GetCurrentProcess().Kill();
                        break;

                    default:
                        XtraMessageBox.Show("Something went wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Process.GetCurrentProcess().Kill();
                        break;
                }
            }
            catch (CryptographicException)
            {
                XtraMessageBox.Show("Invalid Encryption key!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
        }
        public bool login(string username, string password, string hwid = null)
        {
            if (hwid == null) hwid = security.HWID();

            if (!is_initialized)
            {
                XtraMessageBox.Show("Please initialize your application first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var values_to_upload = new NameValueCollection
            {
                ["username"] = encryption.encrypt(username, api_enc_key, session_iv),
                ["password"] = encryption.encrypt(password, api_enc_key, session_iv),
                ["hwid"] = encryption.encrypt(hwid, api_enc_key, session_iv),
                ["program_key"] = encryption.saltStr(program_key),
                ["timestamp"] = encryption.saltStr(DateTime.Now.ToString()),
                ["sessid"] = encryption.saltStr(session_iv)
            };

            var response = do_request("login", values_to_upload);
            errCheck(response);
            security.End();
            response = encryption.decrypt(response, api_enc_key, session_iv);

            var decoded_response = response_decoder.string_to_generic<response_structure>(response);
            
            logged_in = decoded_response.success;
            logged = decoded_response.success;

            if (!logged_in && show_messages)
                XtraMessageBox.Show(decoded_response.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (logged_in)
                load_user_data(decoded_response.user_data);

            return logged_in;
        }

        public bool register(string username, string email, string password, string token, string hwid = null)
        {
            if (hwid == null) hwid = security.HWID();

            if (!is_initialized)
            {
                XtraMessageBox.Show("Please initialize your application first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var values_to_upload = new NameValueCollection
            {
                ["username"] = encryption.encrypt(username, api_enc_key, session_iv),
                ["email"] = encryption.encrypt(email, api_enc_key, session_iv),
                ["password"] = encryption.encrypt(password, api_enc_key, session_iv),
                ["token"] = encryption.encrypt(token, api_enc_key, session_iv),
                ["hwid"] = encryption.encrypt(hwid, api_enc_key, session_iv),
                ["program_key"] = encryption.saltStr(program_key),
                ["timestamp"] = encryption.saltStr(DateTime.Now.ToString()),
                ["sessid"] = encryption.saltStr(session_iv)
            };

            var response = do_request("register", values_to_upload);
            errCheck(response);
            security.End();
            response = encryption.decrypt(response, api_enc_key, session_iv);

            var decoded_response = response_decoder.string_to_generic<response_structure>(response);

            if (!decoded_response.success && show_messages)
                XtraMessageBox.Show(decoded_response.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return decoded_response.success;
        }
        public bool extendSubscription(string username, string token)
        {
            if (!is_initialized)
            {
                XtraMessageBox.Show("Please initialize your application first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var values_to_upload = new NameValueCollection
            {
                ["username"] = encryption.encrypt(username, api_enc_key, session_iv),
                ["token"] = encryption.encrypt(token, api_enc_key, session_iv),
                ["program_key"] = encryption.saltStr(program_key),
                ["timestamp"] = encryption.saltStr(DateTime.Now.ToString()),
                ["sessid"] = encryption.saltStr(session_iv)
            };

            var response = do_request("extend", values_to_upload);
            errCheck(response);
            security.End();
            response = encryption.decrypt(response, api_enc_key, session_iv);

            var decoded_response = response_decoder.string_to_generic<response_structure>(response);

            if (!decoded_response.success && show_messages)
                XtraMessageBox.Show(decoded_response.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return decoded_response.success;
        }
        public string variable(string name)
        {
            if (!is_initialized)
            {
                XtraMessageBox.Show("The program wasn't initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "not_initialized";
            }

            if (!logged_in)
            {
                XtraMessageBox.Show("You can only grab server sided variables after being logged in!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "not_logged_in";
            }

            var values_to_upload = new NameValueCollection
            {
                ["var_name"] = encryption.encrypt(name, api_enc_key, session_iv),
                ["program_key"] = encryption.saltStr(program_key),
                ["timestamp"] = encryption.saltStr(DateTime.Now.ToString()),
                ["sessid"] = encryption.saltStr(session_iv)
            };

            var response = do_request("var", values_to_upload);
            errCheck(response);
            security.End();
            response = encryption.decrypt(response, api_enc_key, session_iv);

            var decoded_response = response_decoder.string_to_generic<response_structure>(response);

            if (!decoded_response.success && show_messages)
                XtraMessageBox.Show(decoded_response.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return decoded_response.response;
        }
        private string do_request(string type, NameValueCollection post_data)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["User-Agent"] = user_agent;
                    security.Start();
                    var raw_response = client.UploadValues(api_endpoint + "?type=" + type, post_data);
                    ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
                    return Encoding.Default.GetString(raw_response);
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Connection failure. Please try again, or contact us for help.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
        public void errCheck(string data)
        {
            if(security.breached)
            {
                XtraMessageBox.Show("Possible malicious activity detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            switch (data)
            {
                case "program_doesnt_exist":
                    XtraMessageBox.Show("The program key you tried to use doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                    break;
                case "program_disabled":
                    XtraMessageBox.Show("Looks like this application is disabled, please try again later!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                    break;
                case "program_banned":
                    XtraMessageBox.Show("This application has been banned for violating the TOS" + Environment.NewLine + "Contact us at support@authguard.net", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.GetCurrentProcess().Kill();
                    break;
                case null:
                    Process.GetCurrentProcess().Kill();
                    break;
                default:
                    break;
            }
        }
        #region structures
        [DataContract]
        private class response_structure
        {
            [DataMember]
            public bool success { get; set; }

            [DataMember]
            public string response { get; set; }

            [DataMember]
            public string message { get; set; }

            [DataMember(IsRequired = false, EmitDefaultValue = false)]
            public user_data_structure user_data { get; set; }
        }

        [DataContract]
        private class user_data_structure
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public string username { get; set; }

            [DataMember]
            public string email { get; set; }

            [DataMember]
            public string expires { get; set; }

            [DataMember]
            public string level { get; set; }

            [DataMember]
            public string ip { get; set; }

            [DataMember]
            public string hwid { get; set; }

            [DataMember]
            public string lastlogin { get; set; }

            [DataMember]
            public string totalclients { get; set; }
        }
        #endregion
        #region user_data
        public user_data_class user_data = new user_data_class();

        public class user_data_class
        {
            public int id { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string ip { get; set; }
            public string hwid { get; set; }
            public string lastlogin { get; set; }
            public string expires { get; set; }
            public string level { get; set; }
            public string totalclients { get; set; }
        }
        private void load_user_data(user_data_structure data)
        {
            user_data.id = data.id;

            user_data.username = data.username;

            user_data.email = data.email;

            user_data.ip = data.ip;

            user_data.hwid = data.hwid;

            user_data.lastlogin = data.lastlogin;

            user_data.expires = data.expires;

            user_data.level = data.level;

            user_data.totalclients = data.totalclients;
        }
        #endregion

        private string api_endpoint = "https://authguard.net/api/v1/";
        public static string user_agent = "AuthGuard Agent";
        private json_wrapper response_decoder = new json_wrapper(new response_structure());
    }
    public static class appSettings
    {
        public static bool devmode { get; set; }
        public static bool freemode { get; set; }
    }
    public static class security
    {
        public static bool started = false, breached = false;
        public static string Signature(string filename)
        {
            string result;
            using (MD5 mD = MD5.Create())
            {
                using (FileStream fileStream = File.OpenRead(filename))
                {
                    byte[] value = mD.ComputeHash(fileStream);
                    result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
                }
            }
            return result;
        }
        public static void Start()
        {
            string drive = Path.GetPathRoot(Environment.SystemDirectory);
            if (started)
            {
                XtraMessageBox.Show("A session has already been started, please end the previous one!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                using (StreamReader sr = new StreamReader($@"{drive}Windows\System32\drivers\etc\hosts"))
                {
                    string contents = sr.ReadToEnd();
                    if (contents.Contains("api.authguard.net"))
                    {
                        breached = true;
                        XtraMessageBox.Show("DNS redirecting has been detected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Process.GetCurrentProcess().Kill();
                    }
                }
                started = true;
            }
        }
        public static void End()
        {
            if (!started)
            {
                XtraMessageBox.Show("No session has been started, closing for security reasons!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                started = false;
            }
        }
        public static string HWID()
        {
            return WindowsIdentity.GetCurrent().User.Value;
        }
    }
    public static class encryption
    {
        public static string saltStr(string data)
        {
            return encryption.byte_arr_to_str(Encoding.Default.GetBytes(data));
        }
        public static string byte_arr_to_str(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] str_to_byte_arr(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string encrypt_string(string plain_text, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            using (MemoryStream mem_stream = new MemoryStream())
            {
                using (ICryptoTransform aes_encryptor = encryptor.CreateEncryptor())
                {
                    using (CryptoStream crypt_stream = new CryptoStream(mem_stream, aes_encryptor, CryptoStreamMode.Write))
                    {
                        byte[] p_bytes = Encoding.Default.GetBytes(plain_text);

                        crypt_stream.Write(p_bytes, 0, p_bytes.Length);

                        crypt_stream.FlushFinalBlock();

                        byte[] c_bytes = mem_stream.ToArray();

                        return byte_arr_to_str(c_bytes);
                    }
                }
            }
        }

        public static string decrypt_string(string cipher_text, byte[] key, byte[] iv)
        {
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = key;
            encryptor.IV = iv;

            using (MemoryStream mem_stream = new MemoryStream())
            {
                using (ICryptoTransform aes_decryptor = encryptor.CreateDecryptor())
                {
                    using (CryptoStream crypt_stream = new CryptoStream(mem_stream, aes_decryptor, CryptoStreamMode.Write))
                    {
                        byte[] c_bytes = str_to_byte_arr(cipher_text);

                        crypt_stream.Write(c_bytes, 0, c_bytes.Length);

                        crypt_stream.FlushFinalBlock();

                        byte[] p_bytes = mem_stream.ToArray();

                        return Encoding.Default.GetString(p_bytes, 0, p_bytes.Length);
                    }
                }
            }
        }

        public static string iv_key() =>
            Guid.NewGuid().ToString().Substring(0, Guid.NewGuid().ToString().IndexOf("-", StringComparison.Ordinal));

        public static string sha256(string r) =>
            byte_arr_to_str(new SHA256Managed().ComputeHash(Encoding.Default.GetBytes(r)));

        public static string encrypt(string message, string enc_key, string iv)
        {
            byte[] _key = Encoding.Default.GetBytes(sha256(enc_key).Substring(0, 32));

            byte[] _iv = Encoding.Default.GetBytes(sha256(iv).Substring(0, 16));

            return encrypt_string(message, _key, _iv);
        }

        public static string decrypt(string message, string enc_key, string iv)
        {
            byte[] _key = Encoding.Default.GetBytes(sha256(enc_key).Substring(0, 32));

            byte[] _iv = Encoding.Default.GetBytes(sha256(iv).Substring(0, 16));

            return decrypt_string(message, _key, _iv);
        }
    }
    public class json_wrapper
    {
        public static bool is_serializable(Type to_check) =>
            to_check.IsSerializable || to_check.IsDefined(typeof(DataContractAttribute), true);

        public json_wrapper(object obj_to_work_with)
        {
            current_object = obj_to_work_with;

            var object_type = current_object.GetType();

            serializer = new DataContractJsonSerializer(object_type);

            if (!is_serializable(object_type))
                throw new Exception($"the object {current_object} isn't a serializable");
        }

        public string to_json_string()
        {
            using (var mem_stream = new MemoryStream())
            {
                serializer.WriteObject(mem_stream, current_object);

                mem_stream.Position = 0;

                using (var reader = new StreamReader(mem_stream))
                    return reader.ReadToEnd();
            }
        }

        public object string_to_object(string json)
        {
            var buffer = Encoding.Default.GetBytes(json);

            //SerializationException = session expired

            using (var mem_stream = new MemoryStream(buffer))
                return serializer.ReadObject(mem_stream);
        }

        #region extras

        public dynamic string_to_dynamic(string json) =>
            (dynamic)string_to_object(json);

        public T string_to_generic<T>(string json) =>
            (T)string_to_object(json);

        public dynamic to_json_dynamic() =>
            string_to_object(to_json_string());

        #endregion

        private DataContractJsonSerializer serializer;

        private object current_object;
    }
}