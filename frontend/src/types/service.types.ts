<artifact identifier="service-types" type = "application/vnd.ant.code" language = "typescript" title = "Service Types" >
export interface Service {
    id: string;
    name: string;
    hourlyRate: number;
    currency: string;
    providerId: string;
    providerName: string;
    countryCodes: string[];
    createdAt: string;
    updatedAt: string;
}
export interface CreateServiceDto {
    name: string;
    hourlyRate: number;
    currency: string;
    providerId: string;
    countryCodes?: string[];
}
export interface UpdateServiceDto {
    id: string;
    name: string;
    hourlyRate: number;
    currency: string;
}
export interface ServiceQueryParams {
    pageNumber?: number;
    pageSize?: number;
    searchTerm?: string;
    providerId?: string;
    countryCode?: string;
}
</artifact>