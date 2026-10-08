namespace IPC2_Proy03.Api.Interfaces
{
    public class NitService : INitService
    {
        public bool EsNitValido(string nit)
        {
            //validamos que realmente venga un valor 

            if (string.IsNullOrWhiteSpace(nit))
            {

                return false;


            }

            //eliminamos espacio al incio y al final
            nit = nit.Trim();


            //Debe contener al menos un numero y el digito verificador. 

            if (nit.Length < 2)
            {
                return false;
            }

            //separamos el digito verificador 

            char verficadorRecibido = char.ToUpperInvariant(nit[nit.Length - 1]);

            string numeroBase = nit.Substring(0, nit.Length - 1);


            foreach (char caracter in numeroBase)
            {
                if (caracter < '0' || caracter > '9')
                {
                    return false;
                }
            }

            //el verificadro solomente puede ser 0-9 o K

            if ((verficadorRecibido < '0' || verficadorRecibido > '9') && verficadorRecibido != 'K')
            {
                return false;

            }

            long suma = 0;

            int multiplicador = 2;

            for (int i = numeroBase.Length - 1; i >= 0; i--)
            {
                //convertimos el caracter a numero "7" a 7

                int digito = numeroBase[i] - '0';

                //multiplicamos segun la posiscion 

                suma += (long)digito * multiplicador;

                //aumentamos el multiplicador para el siguente numero 

                multiplicador++;

            }

            //aplicamos la formula para obtener el digito verificador
            //aplicamos el modulo 11 a la suma de los digitos multiplicados por su posicion

            int modulo = (int)(suma % 11);

            //restamos el resultado 11

            int resultado = 11 - modulo;

            //volvemos a aplicar el modulo 11 al resultado

            resultado = resultado % 11;

            //convertimos el resultado a un caracter

            char verificadorCalculado;

            if (resultado == 10)
            {

                verificadorCalculado = 'K';
            }

            else
            {

                //convertimos el resultado a un caracter 

                verificadorCalculado = resultado.ToString()[0];

            }

            //comparamos ambos digitos verificadores 
            // si son iguales true, si son diferentes false 

            return verificadorCalculado == verficadorRecibido;


        }
    }


        }



