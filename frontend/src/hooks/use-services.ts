import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { servicesApi } from "@/api/services.api";
import { CreateServiceDto, UpdateServiceDto, ServiceQueryParams } from "@/types/service-types";
import { toast } from "@/hooks/use-toast";

/**
 * Hook to fetch paginated list of services
 */
export const useServices = (params: ServiceQueryParams = {}) => {
    return useQuery({
        queryKey: ["services", params],
        queryFn: () => servicesApi.getServices(params),
    });
};

/**
 * Hook to fetch a single service by ID
 */
export const useService = (id: string) => {
    return useQuery({
        queryKey: ["service", id],
        queryFn: () => servicesApi.getServiceById(id),
        enabled: !!id,
    });
};

/**
 * Hook to create a new service
 */
export const useCreateService = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: CreateServiceDto) => servicesApi.createService(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["services"] });
            toast({
                title: "Success",
                description: "Service created successfully.",
            });
        },
        onError: (error: any) => {
            toast({
                title: "Error",
                description: error.response?.data?.error || "Failed to create service.",
                variant: "destructive",
            });
        },
    });
};

/**
 * Hook to update an existing service
 */
export const useUpdateService = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (data: UpdateServiceDto) => servicesApi.updateService(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["services"] });
            toast({
                title: "Success",
                description: "Service updated successfully.",
            });
        },
        onError: (error: any) => {
            toast({
                title: "Error",
                description: error.response?.data?.error || "Failed to update service.",
                variant: "destructive",
            });
        },
    });
};

/**
 * Hook to delete a service
 */
export const useDeleteService = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (id: string) => servicesApi.deleteService(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["services"] });
            toast({
                title: "Success",
                description: "Service deleted successfully.",
            });
        },
        onError: (error: any) => {
            toast({
                title: "Error",
                description: error.response?.data?.error || "Failed to delete service.",
                variant: "destructive",
            });
        },
    });
};

/**
 * Hook to assign countries to a service
 */
export const useAssignCountries = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({
            serviceId,
            countryCodes,
        }: {
            serviceId: string;
            countryCodes: string[];
        }) => servicesApi.assignCountries(serviceId, countryCodes),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["services"] });
            toast({
                title: "Success",
                description: "Countries assigned successfully.",
            });
        },
        onError: (error: any) => {
            toast({
                title: "Error",
                description: error.response?.data?.error || "Failed to assign countries.",
                variant: "destructive",
            });
        },
    });
};
