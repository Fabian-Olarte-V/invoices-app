export interface Product {
    id: number;
    name: string;
    unitPrice: number;
    imageBytes?: string;
    ext?: string;
    imageBase64?: string;
}