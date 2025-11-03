<artifact identifier="api-types" type = "application/vnd.ant.code" language = "typescript" title = "API Types" >
export interface ApiResponse<T> {
    isSuccess: boolean;
    value?: T;
    error?: string;
    errorCode?: string;
}
export interface PagedResult<T> {
    items: T[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}
export interface QueryParams {
    pageNumber?: number;
    pageSize?: number;
    searchTerm?: string;
}
</artifact>