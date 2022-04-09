using System;
using System.Threading.Tasks;
using TechnicalAccounting.Contracts;

namespace TechnicalAccounting.ReadModel
{
  public class AccountViewWriter
  {
    IProjectionWriter<string, AccountView> writer;

    public AccountViewWriter(IProjectionWriter<string, AccountView> writer)
    {
      this.writer = writer;
    }

    public async Task When(AccountRegistered e)
    {
      await writer.Add(e.AccountId.Id, new AccountView
      {
        AccountId = e.AccountId.Id,
        PolicyId = e.AccountId.PolicyId.Id,
        BenefitId = e.AccountId.BenefitId.Id,
        SliceId = e.AccountId.SliceId.Id,
        TimeStampLastEntry = e.Timestamp
      });
    }

    public async Task When(AccountDebited e)
    {
      await writer.Update(e.AccountId.Id, view =>
      {
        view.VersionLastEntry = e.Version;
        view.AccountTransactionIdLastEntry = e.AccountTransactionId;
        view.PostingRuleCodeLastEntry = e.PostingRuleCode;
        view.ValueDateLastEntry = e.ValueDate;
        view.TimeStampLastEntry = e.Timestamp;
        view.Balance += e.Amount;
      });
    }
    public async Task When(AccountCredited e)
    {
      await writer.Update(e.AccountId.Id, view =>
      {
        view.VersionLastEntry = e.Version;
        view.AccountTransactionIdLastEntry = e.AccountTransactionId;
        view.PostingRuleCodeLastEntry = e.PostingRuleCode;
        view.ValueDateLastEntry = e.ValueDate;
        view.TimeStampLastEntry = e.Timestamp;
        view.Balance -= e.Amount;
      });
    }
  }
}