import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { providersApi } from '@/api/providers.api';
import { CreateProviderDto, UpdateProviderDto, ProviderQueryParams } from '@/types/provider.types';
import { toast } from '@/hooks/use-toast';
export const useProviders = (params: ProviderQueryParams = {}) => {
    return useQuery({
        queryKey: ['providers', params],
        queryFn: () => providersApi.getProviders(params),
    });
};
export const useProvider = (id: string) => {
    return useQuery({
        queryKey: ['provider', id],
        queryFn: () => providersApi.getProviderById(id),
        enabled: !!id,
    });
};
export const useCreateProvider = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (data: CreateProviderDto) => providersApi.createProvider(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['providers'] });
            toast({
                title: 'Success',
                description: 'Provider created successfully',
            });
        },
        onError: (error: any) => {
            toast({
                title: 'Error',
                description: error.response?.data?.error || 'Failed to create provider',
                variant: 'destructive',
            });
        },
    });
};
export const useUpdateProvider = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (data: UpdateProviderDto) => providersApi.updateProvider(data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['providers'] });
            toast({
                title: 'Success',
                description: 'Provider updated successfully',
            });
        },
        onError: (error: any) => {
            toast({
                title: 'Error',
                description: error.response?.data?.error || 'Failed to update provider',
                variant: 'destructive',
            });
        },
    });
};
export const useDeleteProvider = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (id: string) => providersApi.deleteProvider(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['providers'] });
            toast({
                title: 'Success',
                description: 'Provider deleted successfully',
            });
        },
        onError: (error: any) => {
            toast({
                title: 'Error',
                description: error.response?.data?.error || 'Failed to delete provider',
                variant: 'destructive',
            });
        },
    });
};
export const useToggleProviderStatus = () => {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (id: string) => providersApi.toggleProviderStatus(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['providers'] });
            toast({
                title: 'Success',
                description: 'Provider status updated',
            });
        },
    });
};
