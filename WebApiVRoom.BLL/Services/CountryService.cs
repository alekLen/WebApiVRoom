﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Infrastructure;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using AutoMapper;

namespace WebApiVRoom.BLL.Services
{
    public class CountryService : ICountryService
    {
        IUnitOfWork Database { get; set; }

        public CountryService(IUnitOfWork database)
        {
            Database = database;
        }
        public async Task AddCountry(CountryDTO countryDTO)
        {
            try
            {
                Country country = new Country();

                country.Id = countryDTO.Id;
                country.Name = countryDTO.Name;
                country.CountryCode = countryDTO.CountryCode;
                

                await Database.Countries.Add(country);
               
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DeleteCountry(int id)
        {
            try
            {
                await Database.Countries.Delete(id);
               
            }
            catch { }
        }

        public async Task<CountryDTO> GetByName(string name)
        {
            var a = await Database.Countries.GetByName(name);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            CountryDTO country = new CountryDTO();
            country.Id = a.Id;
            country.Name = a.Name;
            country.CountryCode = a.CountryCode;

            

            return country;
        }

        public async Task<CountryDTO> GetCountry(int id)
        {
            var a = await Database.Countries.GetById(id);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            CountryDTO country = new CountryDTO();
            country.Id = a.Id;
            country.Name = a.Name;
            country.CountryCode = a.CountryCode;

            

            return country;
        }

        public async Task<CountryDTO> GetCountryByCountryCode(string code)
        {
            var a = await Database.Countries.GetByCountryCode(code);

            if (a == null)
                throw new ValidationException("Wrong country!", "");

            CountryDTO country = new CountryDTO();
            country.Id = a.Id;
            country.Name = a.Name;
            country.CountryCode = a.CountryCode;

           
            return country;
        }

        public async Task<CountryDTO> UpdateCountry(CountryDTO countryDTO)
        {
            Country country = await Database.Countries.GetById(countryDTO.Id);

            try
            {
                country.Id = countryDTO.Id;
                country.Name = countryDTO.Name;
                country.CountryCode = countryDTO.CountryCode;
               

                await Database.Countries.Update(country);

                return countryDTO;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CountryDTO>> GetAllCountries()
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Country, CountryDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Country>, IEnumerable<CountryDTO>>(await Database.Countries.GetAll());
            }
            catch { return null; }
        }

        public async Task<IEnumerable<CountryDTO>> GetAllPaginated(int pageNumber, int pageSize)
        {
            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Country, CountryDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode));
                });

                var mapper = new Mapper(config);
                return mapper.Map<IEnumerable<Country>, IEnumerable<CountryDTO>>(await Database.Countries.GetAllPaginated(pageNumber, pageSize));
            }
            catch { return null; }
        }
    }
}
