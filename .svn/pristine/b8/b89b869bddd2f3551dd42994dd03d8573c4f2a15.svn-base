using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Certification.Model
{
    public enum FlowStatus
    {
        // below are build-in / predefined status for all Jabil WUX. 
        // Do NOT change it.
        Created = 0,
        Returned = 555,
        Closed = 999,
        Rejected = 444,
        Canceled = 777,

        // toDo:  add the status here base on the state diagram, 10~49
        Submitted = 11,
        Approved = 12,
    }

    public enum ActionType
    {
        // Below are pre-defined action. Not Allow to change.
        Create = 0,
        Approve = 1,
        Reject = 2,
        Submit = 3,
        Return = 4,
        NoResponse = 5,
        Cancel = 6,

        // toDo: add actions here, 10~49
        Commit = 10,
        Forward = 20
    }

    public enum RoleType
    {
        Requestor = 10,
        // toDo: add role here, 11~49

        Buyer = 11,
        SME = 12,
        IC = 13,
        Fin = 14,
        TE = 15,
        ME = 16,
        QE = 17,
        MFG = 18,
        HR = 19,
        Planner = 20,
        Approver = 30,
        Eng = 40,
        Admin = 99
    }
}