using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaginRovers
{
    class LidgrenWorkThread
    {
        Networking Communication = new Networking();
        bool Receiver = true;
        long whatever = 0;
        public void SyncComps()
        {
            while (!_shouldStop)
            {
                Communication.Receive();
                whatever++;
            }
        }
        
        public void RequestStop()
        {
            _shouldStop = true;
        }

        private volatile bool _shouldStop;
    }
}