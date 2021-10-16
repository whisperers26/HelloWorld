using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q4_ClassLib
{
    public abstract class Phone
    {
        private string phoneNumber;
        public string address;

        public string PhoneNumber
        {
            get => phoneNumber;
            set => phoneNumber = value;
        }

        public abstract void Connect();

        public abstract void Disconnect();
    }

    public interface PhoneInterface
    {
        void Answer();
        void MakeCall();
        void HangUp();
    }

    public class RotaryPhone : Phone, PhoneInterface
    {
        public void Answer()
        {

        }

        public void MakeCall()
        {

        }

        public void HangUp()
        {

        }

        public override void Connect()
        {
            
        }

        public override void Disconnect()
        {
            
        }
    }

    public class PushButtonPhone : Phone, PhoneInterface
    {
        public void Answer()
        {

        }

        public void MakeCall()
        {

        }

        public void HangUp()
        {

        }

        public override void Connect()
        {

        }

        public override void Disconnect()
        {

        }
    }

    public class Tardis : RotaryPhone
    {
        private bool sonicScrewdriver;
        private byte whichDrWho;
        private string femaleSideKick;
        public double exteriorSurfaceArea;
        public double interiorVolume;

        public byte WhichDrWho
        {
            get => whichDrWho;
        }

        private string FemaleSideKick
        {
            get => femaleSideKick;
        }

        public void TimeTravel()
        {

        }

        public static bool operator ==(Tardis a, Tardis b) => a.whichDrWho == b.whichDrWho;

        public static bool operator !=(Tardis a, Tardis b) => a.whichDrWho != b.whichDrWho;

        public static bool operator <(Tardis a, Tardis b)
        {
            if (a.whichDrWho == 10) return false;
            else if (b.whichDrWho == 10 && a.whichDrWho != 10) return true;
            else return a.whichDrWho < b.whichDrWho;
        }

        public static bool operator >(Tardis a, Tardis b) => !(a < b) && (a != b);

        public static bool operator >=(Tardis a, Tardis b) => !(a < b);

        public static bool operator <=(Tardis a, Tardis b) => !(a > b);
    }

    public class PhoneBooth : PushButtonPhone
    {
        private bool superMan;
        public double costPerCall;
        public bool phoneBook;

        public void OpenDoor()
        {

        }

        public void CloseDoor()
        {

        }
    }
}
