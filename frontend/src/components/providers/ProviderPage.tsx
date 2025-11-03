import { useProviders } from "@/hooks/use-provider.ts";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Button } from "@/components/ui/button";

export function ProviderPage() {
    const { data, isLoading, isError } = useProviders({ pageNumber: 1, pageSize: 10 });

    if (isLoading) return <p>Loading providers...</p>;
    if (isError) return <p>Error loading providers</p>;

    const providers = data?.value?.items ?? [];

    return (
        <div className="p-6">
            <h1 className="text-2xl font-semibold mb-4">Providers</h1>
            <Table>
                <TableHeader>
                    <TableRow>
                        <TableHead>Name</TableHead>
                        <TableHead>NIT</TableHead>
                        <TableHead>Email</TableHead>
                        <TableHead>Status</TableHead>
                        <TableHead></TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {providers.map((p) => (
                        <TableRow key={p.id}>
                            <TableCell>{p.name}</TableCell>
                            <TableCell>{p.nit}</TableCell>
                            <TableCell>{p.email}</TableCell>
                            <TableCell>{p.isActive ? "Active" : "Inactive"}</TableCell>
                            <TableCell>
                                <Button variant="outline">Edit</Button>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </div>
    );
}
