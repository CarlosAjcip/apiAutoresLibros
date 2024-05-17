using apiAutoresLibros.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace apiAutoresLibros.Servicios
{
    public class HashService
    {
        public ResultadoHash Hash(string TextoPlano)
        {
            var sal = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(sal);
            }
            return Hash(TextoPlano, sal);
        }
        public ResultadoHash Hash(string TextoPlano, byte[] sal)
        {
            var llaveDerivada = KeyDerivation.Pbkdf2(password: TextoPlano, salt: sal, prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000, numBytesRequested: 32);

            var hash = Convert.ToBase64String(llaveDerivada);

            return new ResultadoHash()
            {
                Hash = hash,
                Sal = sal
            };
        }
    }
}
