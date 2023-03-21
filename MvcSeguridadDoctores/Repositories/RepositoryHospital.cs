using Microsoft.EntityFrameworkCore;
using MvcSeguridadDoctores.Data;
using MvcSeguridadDoctores.Models;
using System.Runtime.CompilerServices;

namespace MvcSeguridadDoctores.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<Doctor> FindDoctorAsync(int iddoctor)
        {
            Doctor doctor =
                await this.context.Doctores.FirstOrDefaultAsync
                (x => x.IdDoctor == iddoctor);
            return doctor;
        }

        public async Task<List<Enfermo>> GetEnfemosAsync() 
        {
            return await this.context.Enfermos.ToListAsync();
        }

        public async Task<Enfermo> FindEnfermoAsync(int inscripcion)
        {
            Enfermo enfermo =
                await this.context.Enfermos.FirstOrDefaultAsync
                (x => x.Inscripcion == inscripcion);
            return enfermo;
        }

        public async Task DeleteEnfermoAsync(int inscripcion)
        {
            Enfermo enfermo = await this.FindEnfermoAsync(inscripcion);
            if (enfermo != null)
            {
                this.context.Enfermos.Remove(enfermo);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<Doctor> ExisteDoctor(string apellido, int iddoctor)
        {
            Doctor doctor =
                await this.context.Doctores.Where(x => x.Apellido == apellido
                && x.IdDoctor == iddoctor).FirstOrDefaultAsync();
            return doctor;
        }
    }
}
