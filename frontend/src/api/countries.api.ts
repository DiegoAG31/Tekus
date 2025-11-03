import apiClient from "./apiClient";
import { ApiResponse } from "@/types/api-types";
import { Country } from "@/types/country.types";

export const countriesApi = {
    // Get all countries
    getCountries: async (): Promise<ApiResponse<Country[]>> => {
        const response = await apiClient.get("/countries");
        return response.data;
    },

    // Get country by code
    getCountryByCode: async (code: string): Promise<ApiResponse<Country>> => {
        const response = await apiClient.get(`/countries/${code}`);
        return response.data;
    },
};
