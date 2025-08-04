using Auth.Application.DTOs;
using Auth.Domain.Entities;
using Mapster;

namespace Auth.Application.Mappings;

public class AuthMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Mapeamento de ApplicationUser para UserDto
        //config.NewConfig<ApplicationUser, UserDto>()
        //    .Map(dest => dest.Id, src => src.Id)
        //    .Map(dest => dest.Email, src => src.Email)
        //    .Map(dest => dest.Nome, src => src.Nome)
        //    .Map(dest => dest.DataNascimento, src => src.DataNascimento)
        //    .Map(dest => dest.CPF, src => src.CPF)
        //    .Map(dest => dest.Telefone, src => src.Telefone)
        //    .Map(dest => dest.Genero, src => src.Genero)
        //    .Map(dest => dest.Cidade, src => src.Cidade)
        //    .Map(dest => dest.Estado, src => src.Estado)
        //    .Map(dest => dest.CEP, src => src.CEP)
        //    .Map(dest => dest.Foto, src => src.Foto)
        //    .Map(dest => dest.DataCadastro, src => src.DataCadastro)
        //    .Map(dest => dest.Ativo, src => src.Ativo)
        //    .Map(dest => dest.Roles, src => new List<string>()); // Será preenchido pelo serviço

        //// Mapeamento de RegisterRequestDto para ApplicationUser
        //config.NewConfig<RegisterRequestDto, ApplicationUser>()
        //    .Map(dest => dest.UserName, src => src.Email)
        //    .Map(dest => dest.Email, src => src.Email)
        //    .Map(dest => dest.Nome, src => src.Nome)
        //    .Map(dest => dest.DataNascimento, src => src.DataNascimento)
        //    .Map(dest => dest.CPF, src => src.CPF)
        //    .Map(dest => dest.Telefone, src => src.Telefone)
        //    .Map(dest => dest.Genero, src => src.Genero)
        //    .Map(dest => dest.Cidade, src => src.Cidade)
        //    .Map(dest => dest.Estado, src => src.Estado)
        //    .Map(dest => dest.CEP, src => src.CEP)
        //    .Map(dest => dest.Foto, src => src.Foto)
        //    .Map(dest => dest.DataCadastro, src => DateTime.UtcNow)
        //    .Map(dest => dest.Ativo, src => true)
        //    .Map(dest => dest.EmailConfirmed, src => true)
        //    .Ignore(dest => dest.Id)
        //    .Ignore(dest => dest.NormalizedUserName)
        //    .Ignore(dest => dest.NormalizedEmail)
        //    .Ignore(dest => dest.PasswordHash)
        //    .Ignore(dest => dest.SecurityStamp)
        //    .Ignore(dest => dest.ConcurrencyStamp)
        //    .Ignore(dest => dest.PhoneNumber)
        //    .Ignore(dest => dest.PhoneNumberConfirmed)
        //    .Ignore(dest => dest.TwoFactorEnabled)
        //    .Ignore(dest => dest.LockoutEnd)
        //    .Ignore(dest => dest.LockoutEnabled)
        //    .Ignore(dest => dest.AccessFailedCount)
        //    .Ignore(dest => dest.RefreshToken)
        //    .Ignore(dest => dest.RefreshTokenExpiryTime);

        //// Mapeamento de LoginRequestDto para ApplicationUser (apenas para validação)
        //config.NewConfig<LoginRequestDto, ApplicationUser>()
        //    .Map(dest => dest.Email, src => src.Email)
        //    .Ignore(dest => dest.Id)
        //    .Ignore(dest => dest.UserName)
        //    .Ignore(dest => dest.NormalizedUserName)
        //    .Ignore(dest => dest.NormalizedEmail)
        //    .Ignore(dest => dest.PasswordHash)
        //    .Ignore(dest => dest.SecurityStamp)
        //    .Ignore(dest => dest.ConcurrencyStamp)
        //    .Ignore(dest => dest.PhoneNumber)
        //    .Ignore(dest => dest.PhoneNumberConfirmed)
        //    .Ignore(dest => dest.TwoFactorEnabled)
        //    .Ignore(dest => dest.LockoutEnd)
        //    .Ignore(dest => dest.LockoutEnabled)
        //    .Ignore(dest => dest.AccessFailedCount)
        //    .Ignore(dest => dest.Nome)
        //    .Ignore(dest => dest.DataNascimento)
        //    .Ignore(dest => dest.DataCadastro)
        //    .Ignore(dest => dest.Ativo)
        //    .Ignore(dest => dest.EmailConfirmed)
        //    .Ignore(dest => dest.RefreshToken)
        //    .Ignore(dest => dest.RefreshTokenExpiryTime);
    }
}