using FoodSustain.Entities;
using SimpleSpritePacker;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodSustain.Util
{
    internal static class Helpers
    {
        public static int ContarSequencia(Queue<long> numeros)
        {
            long[] arrayNumeros = numeros.ToArray();
            long ultimoNumero = arrayNumeros[arrayNumeros.Length - 1];
            bool operadoringress = arrayNumeros.Length == 1 ? true : arrayNumeros[arrayNumeros.Length - 2] == ultimoNumero;
            int contador = 0;

            if (operadoringress) {
                for (long i = arrayNumeros.Length - 1; i >= 0; i--)
                {
                    if (arrayNumeros[i] == ultimoNumero)
                        contador++;
                    else
                        break;
                }
            }else
            {
                for (long i = arrayNumeros.Length - 1; i >= 0; i--)
                {
                    if (arrayNumeros[i] != ultimoNumero)
                        contador--;
                    else
                        break;
                    
                }
                contador /= 4;
            }
            return contador;
        }
    }
}
