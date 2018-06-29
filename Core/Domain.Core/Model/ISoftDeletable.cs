namespace Nagnoi.SiC.Domain.Core.Model
{
    public interface ISoftDeletable
    {
        bool? Hidden { get; set; }
    }
}