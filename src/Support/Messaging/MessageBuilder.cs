using Plisky.Plumbing;

namespace FlimFlam {

    public static class Messages {

        public enum KnownMessages {

            ///<summary>Flimflam screen navigation message to show the details view.</summary>
            ShowDetailsView = 0,

            ///<summary>Flimflam screen navigation message to go to the home screen.</summary>
            ShowHomeView = 1
        }

        public static int Get(KnownMessages km) {
            return (int)km;
        }

        public static void Raise(KnownMessages km) {
            Hub.Current.Launch((int)km);
        }
    }
}