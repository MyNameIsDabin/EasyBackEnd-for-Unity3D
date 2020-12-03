using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;

namespace BackEndExtends
{
    public class EasyBackEnd : Singleton<EasyBackEnd>
    {
        public string GuestId { get; set; } = "";

        public void Init(Action successCallback, Action failureCallback)
        {
            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                {
                    successCallback.Invoke();
                }
                else
                {
                    failureCallback.Invoke();
                }
            });
        }

        public string GuestLogin()
        {
            Backend.BMember.GuestLogin();
            GuestId = Backend.BMember.GetGuestID();
            return GuestId;
        }
    }
}
