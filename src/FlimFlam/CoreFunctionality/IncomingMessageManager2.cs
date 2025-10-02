
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plisky.Plumbing;

namespace Plisky.FlimFlam;

public class IncomingMessageManager2 : IncomingMessageManager {
    protected Feature ft;
    protected OdsProcessGatherer odsProcessGatherer;

    public IncomingMessageManager2(OdsProcessGatherer pg) : base() {
        ft = Feature.GetFeatureByName("Bilge-OdsOOP");
        odsProcessGatherer = pg;
    }


    public override void ActivateODSGatherer() {
        if (ft.Active) {
            odsProcessGatherer.StartOdsGathererProcess();
        } else {
            base.ActivateODSGatherer();
        }        
    }

    public override void DeactivateODSGatherer() {
        if (ft.Active) {
            odsProcessGatherer.StopOdsGathererProcess();
        } else {
            base.DeactivateODSGatherer();
        }
    }



}

