﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoMarket.Domain.Entities
{
    public enum PaymentMethod
    {
        Pix,
        DigitalWallet,
        CreditCard,
        DebitCard,
        BankSlip
    }
}
