using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Reporting.Domain.Models;
using Lykke.Service.Reporting.Domain.Repositories;
using Lykke.Service.Reporting.Domain.Services;
using Lykke.Service.Reporting.DomainServices.Utils;

namespace Lykke.Service.Reporting.DomainServices.Services
{
    public class TransactionReportReader : ITransactionReportReader 
    {
        private readonly ITransactionReportRepository _transactionReportRepository;

        public TransactionReportReader(ITransactionReportRepository transactionReportRepository)
        {
            _transactionReportRepository = transactionReportRepository;
        }

        public async Task<TransactionReportResult> GetPaginatedAsync(
            int currentPage, int pageSize,
            DateTime from, DateTime to)
        {
            var (skip, take) = PagingUtils.GetNextPageParameters(currentPage, pageSize);

            var reports = await _transactionReportRepository.GetPaginatedAsync(skip, take, from, to);

            return new TransactionReportResult
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = reports.Count,
                TransactionReports = reports
            };
        }

        public async Task<IReadOnlyList<TransactionReport>> GetLimitedAsync(
            DateTime from, DateTime to, int limit)
        {
            if (limit <= 0) throw new ArgumentException();

            var reports = await _transactionReportRepository.GetLimitedAsync(from, to, limit);
            return reports;
        }
    }
}
