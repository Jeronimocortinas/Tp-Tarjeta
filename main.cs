using System;

class Program{
    static void Main(string[] args){
        Tarjeta tarjeta = null;
        Colectivo colectivo = new Colectivo();

        while (true){
            Console.WriteLine("\n--- Sistema de Tarjetas ---");
            Console.WriteLine("1. Crear tarjeta normal");
            Console.WriteLine("2. Crear tarjeta de medio boleto");
            Console.WriteLine("3. Crear tarjeta de franquicia completa");
            Console.WriteLine("4. Seleccionar línea urbana o interurbana");
            Console.WriteLine("5. Cargar saldo");
            Console.WriteLine("6. Pagar boleto");
            Console.WriteLine("7. Ver saldo");
            Console.WriteLine("8. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.Clear();
            try{
                switch (opcion){
                    case "1":
                        Console.Write("Ingrese saldo inicial para la tarjeta normal: ");
                        decimal saldoNormal = decimal.Parse(Console.ReadLine());
                        tarjeta = new TarjetaNormal(saldoNormal);
                        Console.WriteLine("Tarjeta normal creada.");
                        break;
                    case "2":
                        Console.Write("Ingrese saldo inicial para la tarjeta de medio boleto: ");
                        decimal saldoMedio = decimal.Parse(Console.ReadLine());
                        tarjeta = new MedioBoleto(saldoMedio);
                        Console.WriteLine("Tarjeta de medio boleto creada.");
                        break;
                    case "3":
                        Console.Write("Ingrese saldo inicial para la tarjeta de franquicia completa: ");
                        decimal saldoCompleto = decimal.Parse(Console.ReadLine());
                        tarjeta = new FranquiciaCompleta(saldoCompleto);
                        Console.WriteLine("Tarjeta de franquicia completa creada.");
                        break;
                    case "4":
                        Console.WriteLine("Seleccione tipo de línea:");
                        Console.WriteLine("1. Urbana ($1200)");
                        Console.WriteLine("2. Interurbana ($2500)");
                        string tipoLinea = Console.ReadLine();
                        colectivo = tipoLinea == "2" ? new Colectivo(true) : new Colectivo();
                        Console.WriteLine(tipoLinea == "2" ? "Línea interurbana seleccionada." : "Línea urbana seleccionada.");
                        break;
                    case "5":
                        if (tarjeta == null){
                            Console.WriteLine("Primero debe crear una tarjeta.");
                            break;
                        }
                        Console.Write("Montos de carga disponibles:2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000\nIngrese monto de carga: ");
                        
                        decimal montoCarga = decimal.Parse(Console.ReadLine());
                        tarjeta.CargarSaldo(montoCarga);
                        Console.WriteLine("Saldo cargado exitosamente.");
                        break;
                    case "6":
                        if (tarjeta == null){
                            Console.WriteLine("Primero debe crear una tarjeta.");
                            break;
                        }
                        var boleto = colectivo.PagarCon(tarjeta);
                        Console.WriteLine(boleto.ToString());
                        break;
                    case "7":
                        Console.WriteLine($"Saldo actual: {tarjeta.Saldo}");
                        break;
                    case "8":
                        Console.WriteLine("Saliendo del sistema...");
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            catch (Exception ex){
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}