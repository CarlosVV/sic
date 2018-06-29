namespace Nagnoi.SiC.Domain.Core.Model
{
    public enum AuditObjectType : int
    {
        None = 0,
        Generic = 401,
        AddressType = 402,
        ApplyTo = 403,
        Beneficiary = 404,
        CivilStatus = 405,
        Gender = 406,
        InternetType = 407,
        ActivityLogType = 408,
        Setting = 409,
        Entity = 410,
        RelationshipType = 411,
        City = 412,
        Clinic = 413,
        Court = 414,
        Region = 415,
        State = 416,
        PaymentClass = 417,
        PaymentConcept = 418,
        PaymentMonthlyConcept = 419,
        PaymentPayment = 420,
        PaymentStatus = 421,
        PaymentThirdPartySchedule = 422,
        PaymentTransferType = 423,
        SicAdjustmentReason = 424,
        SicCancellation = 425,
        SicCase = 426,
        SicCaseDetail = 427,
        SicDecision = 428,
        SicDecisionDetail = 429,
        SicTransaction = 430,
        SicTransactionDetail = 431,
        SicTransactionType = 432,
        SimeraBeneficiary = 433,
        SimeraTransaction = 434
    }

}