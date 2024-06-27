﻿using Microsoft.EntityFrameworkCore;
using Revenue_Recognition_System.Context;
using Revenue_Recognition_System.Models;

namespace Revenue_Recognition_System.Repositories;

public class IndividualRepository : BaseRepository<Individual>, IIndividualRepository
{
    public IndividualRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Individual?> GetByPesel(string pesel)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.Pesel == pesel);
    }
}