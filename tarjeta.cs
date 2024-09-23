using System;

public class Tarjeta
{
    public decimal Saldo { get; protected set; }
    private readonly decimal SaldoMaximo = 9900;
    private readonly decimal SaldoNegativoPermitido = -480;
    private readonly decimal[] CargasAceptadas = { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };
    public Tarjeta(decimal saldoInicial)
    {
        if (saldoInicial <= SaldoMaximo)
        {
            Saldo = saldoInicial;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Saldo inicial excede el saldo máximo permitido.");
        }
    }
    public virtual bool PuedePagar(decimal monto)
    {
        return Saldo - monto >= SaldoNegativoPermitido;
    }
    public virtual void DescontarSaldo(decimal monto)
    {
        if (PuedePagar(monto))
        {
            Saldo -= monto;
        }
        else
        {
            throw new InvalidOperationException("La tarjeta no puede tener menos saldo que el permitido (-480).");
        }
    }
    public void CargarSaldo(decimal monto)
    {
        if (Array.Exists(CargasAceptadas, carga => carga == monto))
        {
            decimal deuda = 0;
            if (Saldo < 0)
            {
                deuda = -Saldo;
                if (monto >= deuda)
                {
                    monto -= deuda;
                    Saldo = 0;
                }
                else
                {
                    Saldo += monto;
                    monto = 0;
                }
            }
            if (monto > 0)
            {
                if (Saldo + monto <= SaldoMaximo)
                {
                    Saldo += monto;
                }
                else
                {
                    throw new InvalidOperationException("La carga excede el saldo máximo permitido.");
                }
            }
        }
        else
        {
            throw new ArgumentException("Monto de carga no aceptado.");
        }
    }
}
