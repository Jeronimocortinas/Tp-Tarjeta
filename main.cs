using System;

class Program
{
    static void Main(string[] args)
    {
        Tarjeta tarjeta = null;
        Colectivo colectivo = new Colectivo(940m); 
        while (true)
        {
            Console.WriteLine("\n--- Sistema de Tarjetas ---");
            Console.WriteLine("1. Crear tarjeta normal");
            Console.WriteLine("2. Crear tarjeta de medio boleto");
            Console.WriteLine("3. Crear tarjeta de franquicia completa");
            Console.WriteLine("4. Cargar saldo");
            Console.WriteLine("5. Pagar boleto");
            Console.WriteLine("6. Ver saldo");
            Console.WriteLine("7. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();
            try
            {
                switch (opcion)
                {
                    case "1":
                        Console.Write("Ingrese saldo inicial para la tarjeta normal: ");
                        decimal saldoNormal = decimal.Parse(Console.ReadLine());
                        tarjeta = new Tarjeta(saldoNormal);
                        Console.WriteLine("Tarjeta normal creada.");
                        break;
                    case "2":
                        Console.Write("Ingrese saldo inicial para la tarjeta de medio boleto: ");
                        decimal saldoMedio = decimal.Parse(Console.ReadLine());
                        tarjeta = new MedioBoleto(saldoMedio);
                        Console.WriteLine("Tarjeta de medio boleto creada.");
                        break;
                    case "3":
                        tarjeta = new FranquiciaCompleta(0m); 
                        Console.WriteLine("Tarjeta de franquicia completa creada.");
                        break;
                    case "4":
                        if (tarjeta == null)
                        {
                            Console.WriteLine("Primero debe crear una tarjeta.");
                            break;
                        }
                        Console.Write("Ingrese monto a cargar: ");
                        decimal montoCarga = decimal.Parse(Console.ReadLine());
                        tarjeta.CargarSaldo(montoCarga);
                        Console.WriteLine($"Saldo después de la carga: {tarjeta.Saldo}");
                        break;
                    case "5":
                        if (tarjeta == null)
                        {
                            Console.WriteLine("Primero debe crear una tarjeta.");
                            break;
                        }
                        Boleto boleto = colectivo.PagarCon(tarjeta);
                        Console.WriteLine(boleto);
                        break;
                    case "6":
                        if (tarjeta == null)
                        {
                            Console.WriteLine("Primero debe crear una tarjeta.");
                        }
                        else
                        {
                            Console.WriteLine($"Saldo actual: {tarjeta.Saldo}");
                        }
                        break;

                    case "7":
                        Console.WriteLine("Saliendo del sistema...");
                        return;

                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }
        }
    }
}
