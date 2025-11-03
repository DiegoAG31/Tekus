import apiClient from "./apiClient";
import { ApiResponse, PagedResult } from "@/types/api-types";
import {
    Service,
    CreateServiceDto,
    UpdateServiceDto,
    ServiceQueryParams,
} from "@/types/service.types";

export const servicesApi = {
    // Get paginated services
    getServices: async (
        params: ServiceQueryParams
    ): Promise<ApiResponse<PagedResult<Service>>> => {
        const response = await apiClient.get("/services", { params });
        return response.data;
    },

    // Get service by ID
    getServiceById: async (id: string): Promise<ApiResponse<Service>> => {
        const response = await apiClient.get(`/services/${id}`);
        return response.data;
    },

    // Get services by provider
    getServicesByProvider: async (
        providerId: string
    ): Promise<ApiResponse<Service[]>> => {
        const response = await apiClient.get(`/services/provider/${providerId}`);
        return response.data;
    },

    // Create service
    createService: async (
        data: CreateServiceDto
    ): Promise<ApiResponse<string>> => {
        const response = await apiClient.post("/services", data);
        return response.data;
    },

    // Update service
    updateService: async (
        data: UpdateServiceDto
    ): Promise<ApiResponse<boolean>> => {
        const response = await apiClient.put(`/services/${data.id}`, data);
        return response.data;
    },

    // Delete service
    deleteService: async (id: string): Promise<ApiResponse<boolean>> => {
        const response = await apiClient.delete(`/services/${id}`);
        return response.data;
    },

    // Assign countries to service
    assignCountries: async (
        serviceId: string,
        countryCodes: string[]
    ): Promise<ApiResponse<boolean>> => {
        const response = await apiClient.post(`/services/${serviceId}/countries`, {
            countryCodes,
        });
        return response.data;
    },
};
