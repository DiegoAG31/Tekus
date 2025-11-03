<artifact identifier="provider-types" type = "application/vnd.ant.code" language = "typescript" title = "Provider Types" >
export interface Provider {
    id: string;
    nit: string;
    name: string;
    email: string;
    isActive: boolean;
    createdAt: string;
    updatedAt: string;
    customFields: CustomField[];
}
export interface CustomField {
    key: string;
    value: string;
    type: string;
}
export interface CreateProviderDto {
    nit: string;
    name: string;
    email: string;
    customFields?: CustomField[];
}
export interface UpdateProviderDto {
    id: string;
    name: string;
    email: string;
}
export interface ProviderQueryParams {
    pageNumber?: number;
    pageSize?: number;
    searchTerm?: string;
    isActive?: boolean;
}
</artifact>