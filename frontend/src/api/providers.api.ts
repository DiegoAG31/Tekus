import apiClient from "./apiClient";
import { ApiResponse, PagedResult } from "@/types/api-types";
import {
    Provider,
    CreateProviderDto,
    UpdateProviderDto,
    ProviderQueryParams,
} from "@/types/provider.types";

export const providersApi = {
    // Get paginated providers
    getProviders: async (
        params: ProviderQueryParams
    ): Promise<ApiResponse<PagedResult<Provider>>> => {
        const response = await apiClient.get("/providers", { params });
        return response.data;
    },

    // Get provider by ID
    getProviderById: async (id: string): Promise<ApiResponse<Provider>> => {
        const response = await apiClient.get(`/providers/${id}`);
        return response.data;
    },

    // Get provider by NIT
    getProviderByNit: async (nit: string): Promise<ApiResponse<Provider>> => {
        const response = await apiClient.get(`/providers/nit/${nit}`);
        return response.data;
    },

    // Create provider
    createProvider: async (
        data: CreateProviderDto
    ): Promise<ApiResponse<string>> => {
        const response = await apiClient.post("/providers", data);
        return response.data;
    },

    // Update provider
    updateProvider: async (
        data: UpdateProviderDto
    ): Promise<ApiResponse<boolean>> => {
        const response = await apiClient.put(`/providers/${data.id}`, data);
        return response.data;
    },

    // Delete provider
    deleteProvider: async (id: string): Promise<ApiResponse<boolean>> => {
        const response = await apiClient.delete(`/providers/${id}`);
        return response.data;
    },

    // Toggle provider status
    toggleProviderStatus: async (
        id: string
    ): Promise<ApiResponse<boolean>> => {
        const response = await apiClient.patch(`/providers/${id}/toggle-status`);
        return response.data;
    },
};
