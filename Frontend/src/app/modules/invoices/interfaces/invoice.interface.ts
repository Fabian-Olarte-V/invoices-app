export interface Invoice {
    id: number;
    invoiceNumber: string;
    createdDate: Date;
    total: number;
}

export interface InvoiceCreateRequest {
    invoiceData: {
        clientId: number;
        invoiceNumber: string;
        items: InvoiceDetail[];
    };
}

export interface InvoiceDetail {
    productId: number;
    quantity: number;
    unitPrice: number;
}

export interface InvoiceSearchCriteria {
    searchType: 'client' | 'invoice';
    clientId?: number;
    invoiceNumber?: string;
}
