using System.Security.Cryptography;

namespace Utils;

public static class Password {
        
        private static int ITERATION = 100000;
        private static int SALT_SIZE = 16;
        private static int OUTPUT_LEN= 32;

        public static String Hash(string psw) {
                byte[] salt = new Byte[SALT_SIZE]; 
                new Random().NextBytes(salt);
                return Hash(System.Text.Encoding.ASCII.GetBytes(psw),salt);
        }
        
        public static String Hash(byte[] psw, byte[] salt) { //create key
                byte[] key = Rfc2898DeriveBytes.Pbkdf2(psw,
                                                    salt,
                                                    ITERATION,
                                                    HashAlgorithmName.SHA256,
                                                    OUTPUT_LEN);

                Byte[] full = new byte[OUTPUT_LEN + SALT_SIZE];
                Buffer.BlockCopy(key,0,full,0,OUTPUT_LEN);
                Buffer.BlockCopy(salt,0,full,OUTPUT_LEN,SALT_SIZE);
                return Convert.ToBase64String(full); 
        }


        public static bool Validate(string psw, string target) {
                //----------------get salt -----------------//
                byte[] full = Convert.FromBase64String(target);
                byte[] salt = new byte[SALT_SIZE];
                byte[] output = new byte[OUTPUT_LEN];

                Buffer.BlockCopy(full,0,output,0,OUTPUT_LEN);
                Buffer.BlockCopy(full,OUTPUT_LEN,salt,0,SALT_SIZE);


                byte[] key = Rfc2898DeriveBytes.Pbkdf2(psw,
                                                       salt,
                                                       ITERATION,
                                                       HashAlgorithmName.SHA256,
                                                       OUTPUT_LEN);
                
                return CryptographicOperations.FixedTimeEquals(key,output);
        }
}

