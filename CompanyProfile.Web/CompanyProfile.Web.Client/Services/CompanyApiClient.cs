using CompanyProfile.Shared.Common;
using CompanyProfile.Shared.DTOs;
using CompanyProfile.Shared.DTOs.Auth;
using GenericRestHelper.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

namespace CompanyProfile.Web.Client.Services;

public sealed class CompanyApiClient(IRestClientService restClient)
{
    public async Task<bool> LoginAsync(string userName, string password)
    {
        var response = await restClient.PostAsync<LoginRequest, ApiResponse<object>>(
            "api/auth/login",
            new LoginRequest(userName, password));

        return response?.Success == true;
    }

    public async Task<bool> LogoutAsync()
    {
        var response = await restClient.PostAsync<object, ApiResponse<object>>(
            "api/auth/logout",
            new { });

        return response?.Success == true;
    }

    public async Task<CompanyInfoWithTranslationsResponseDto?> GetAllCompanyInfoAsync()
    {
        var response = await restClient.GetAsync<ApiResponse<CompanyInfoWithTranslationsResponseDto>>(
            "api/admin/infoWithTran");

        return response?.Success == true ? response.Data : null;
    }

    public async Task<CompanyInfoResponseDto?> UpdateCompanyInfoAsync(
        string language,
        UpdateCompanyInfoDto data)
    {
        var response = await restClient.PutAsync<UpdateCompanyInfoDto, ApiResponse<CompanyInfoResponseDto>>(
            $"api/admin/{language}/info",
            data);

        return response?.Success == true ? response.Data : null;
    }

    public async Task<ServiceResponseDto?> AddServiceAsync(
        string language,
        CreateServiceDto data)
    {
        var response = await restClient.PostAsync<CreateServiceDto, ApiResponse<ServiceResponseDto>>(
            $"api/admin/{language}/services",
            data);

        return response?.Success == true ? response.Data : null;
    }

    public async Task<ServiceResponseDto?> UpdateServiceAsync(
        int id,
        string language,
        UpdateServiceDto data)
    {
        var response = await restClient.PutAsync<UpdateServiceDto, ApiResponse<ServiceResponseDto>>(
            $"api/admin/{language}/services/{id}", data);
        return response?.Success == true ? response.Data : null;
    }

    public async Task<ApiResponse<ServiceResponseDto>?> UpdateServiceResponseAsync(
        int id,
        string language,
        UpdateServiceDto data)
        => await restClient.PutAsync<UpdateServiceDto, ApiResponse<ServiceResponseDto>>(
            $"api/admin/{language}/services/{id}", data);

    public async Task<ServiceResponseDto?> AddServiceTranslationAsync(
        int id,
        string language,
        CreateServiceTranslationDto data)
    {
        var response = await restClient.PostAsync<CreateServiceTranslationDto, ApiResponse<ServiceResponseDto>>(
            $"api/admin/{language}/services/{id}/translation",
            data);
        return response?.Success == true ? response.Data : null;
    }

    public async Task<ApiResponse<ServiceResponseDto>?> AddServiceTranslationResponseAsync(
        int id,
        string language,
        CreateServiceTranslationDto data)
        => await restClient.PostAsync<CreateServiceTranslationDto, ApiResponse<ServiceResponseDto>>(
            $"api/admin/{language}/services/{id}/translation", data);

    public async Task<bool> DeleteServiceAsync(int id)
    {
        var response = await restClient.DeleteAsync($"api/admin/services/{id}");
        return response;
    }

    public async Task<TeamMemberResponseDto?> AddTeamMemberAsync(
        string language,
        CreateTeamMemberAdminDto data)
    {
        var response = await restClient.PostAsync<CreateTeamMemberAdminDto, ApiResponse<TeamMemberResponseDto>>(
            $"api/admin/{language}/team",
            data);

        return response?.Success == true ? response.Data : null;
    }

    public async Task<TeamMemberResponseDto?> AddTeamMemberWithImageAsync(
        string language,
        string name,
        string role,
        string bio,
        IBrowserFile image)
    {
        using var content = await CreateTeamMemberMultipartAsync(name, role, bio, image);
        var response = await restClient.PostMultipartAsync<ApiResponse<TeamMemberResponseDto>>(
            $"api/admin/{language}/team/upload",
            content);
        return response?.Success == true ? response.Data : null;
    }

    public async Task<TeamMemberResponseDto?> UpdateTeamMemberAsync(
        int id,
        string language,
        UpdateTeamMemberAdminDto data)
    {
        var response = await restClient.PutAsync<UpdateTeamMemberAdminDto, ApiResponse<TeamMemberResponseDto>>(
            $"api/admin/{language}/team/{id}", data);
        return response?.Success == true ? response.Data : null;
    }

    public async Task<TeamMemberResponseDto?> AddTeamMemberTranslationAsync(
        int id,
        string language,
        CreateTeamMemberTranslationDto data)
    {
        var response = await restClient.PostAsync<CreateTeamMemberTranslationDto, ApiResponse<TeamMemberResponseDto>>(
            $"api/admin/{language}/team/{id}/translation", data);
        return response?.Success == true ? response.Data : null;
    }

    public async Task<TeamMemberResponseDto?> UpdateTeamMemberWithImageAsync(
        int id,
        string language,
        string name,
        string role,
        string bio,
        IBrowserFile image)
    {
        using var content = await CreateTeamMemberMultipartAsync(name, role, bio, image);
        var response = await restClient.PutMultipartAsync<ApiResponse<TeamMemberResponseDto>>(
            $"api/admin/{language}/team/{id}/upload",
            content);
        return response?.Success == true ? response.Data : null;
    }

    public async Task<bool> DeleteTeamMemberAsync(int id)
        => await restClient.DeleteAsync($"api/admin/team/{id}");

    public async Task<IReadOnlyList<ContactMessageAdminDto>> GetContactMessagesAsync()
    {
        var response = await restClient.GetAsync<ApiResponse<List<ContactMessageAdminDto>>>("api/admin/contact");
        return response?.Success == true && response.Data is not null ? response.Data : [];
    }

    public async Task<bool> DeleteContactMessageAsync(int id)
        => await restClient.DeleteAsync($"api/admin/contact/{id}");

    public async Task<CompanyInfoResponseDto?> GetCompanyInfoAsync(
        string language,
        CancellationToken cancellationToken = default)
    {
        var response = await restClient.GetAsync<ApiResponse<CompanyInfoResponseDto>>(
            "api/info",
            CreateLanguageHeaders(language));
        return response?.Success == true ? response.Data : null;
    }

    public async Task<IReadOnlyList<ServiceResponseDto>> GetServicesAsync(
        string language,
        CancellationToken cancellationToken = default)
    {
        var response = await restClient.GetAsync<ApiResponse<List<ServiceResponseDto>>>(
            "api/services",
            CreateLanguageHeaders(language));
        return response?.Success == true && response.Data is not null
            ? response.Data
            : [];
    }

    public async Task<ServiceWithTranslationsResponseDto?> GetServiceWithTranslationsAsync(int id)
    {
        var response = await restClient.GetAsync<ApiResponse<ServiceWithTranslationsResponseDto>>(
            $"api/admin/servicesWithAllTrans/{id}");
        return response?.Success == true ? response.Data : null;
    }

    public async Task<IReadOnlyList<TeamMemberResponseDto>> GetTeamMembersAsync(
        string language,
        CancellationToken cancellationToken = default)
    {
        var response = await restClient.GetAsync<ApiResponse<List<TeamMemberResponseDto>>>(
            "api/team",
            CreateLanguageHeaders(language));
        return response?.Success == true && response.Data is not null
            ? response.Data
            : [];
    }

    public async Task<ContactMessageResponseDto?> SendContactMessageAsync(
        CreateContactMessageDto message,
        CancellationToken cancellationToken = default)
    {
        var response = await restClient.PostAsync<CreateContactMessageDto, ApiResponse<ContactMessageResponseDto>>(
            "api/contact",
            message);

        return response?.Success == true ? response.Data : null;
    }

    private static Dictionary<string, string> CreateLanguageHeaders(string language)
        => new() { ["Accept-Language"] = language };

    private static async Task<MultipartFormDataContent> CreateTeamMemberMultipartAsync(
        string name,
        string role,
        string bio,
        IBrowserFile image)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(name), "name");
        content.Add(new StringContent(role), "role");
        content.Add(new StringContent(bio), "bio");

        var stream = image.OpenReadStream(5 * 1024 * 1024);
        var imageContent = new StreamContent(stream);
        imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
        content.Add(imageContent, "image", image.Name);
        return await Task.FromResult(content);
    }
}