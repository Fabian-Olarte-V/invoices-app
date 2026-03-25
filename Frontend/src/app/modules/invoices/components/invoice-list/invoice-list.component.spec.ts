import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';
import { InvoiceListComponent } from './invoice-list.component';

describe('InvoiceListComponent', () => {
  let component: InvoiceListComponent;
  let fixture: ComponentFixture<InvoiceListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvoiceListComponent],
      providers: [FormBuilder]
    }).compileComponents();

    fixture = TestBed.createComponent(InvoiceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('starts in client search mode with clientId required', () => {
    const clientControl = component.searchForm.get('clientId');
    const invoiceControl = component.searchForm.get('invoiceNumber');

    clientControl?.setValue('');
    invoiceControl?.setValue('');
    clientControl?.markAsTouched();
    invoiceControl?.markAsTouched();
    clientControl?.updateValueAndValidity();
    invoiceControl?.updateValueAndValidity();

    expect(component.searchType).toBe('client');
    expect(clientControl?.hasError('required')).toBeTrue();
    expect(invoiceControl?.hasError('required')).toBeFalse();
  });

  it('switches validators when changing to invoice search mode', () => {
    component.searchForm.patchValue({
      clientId: '3'
    });
    component.searchForm.patchValue({
      searchType: 'invoice'
    });

    const clientControl = component.searchForm.get('clientId');
    const invoiceControl = component.searchForm.get('invoiceNumber');
    invoiceControl?.setValue('');
    invoiceControl?.updateValueAndValidity();

    expect(component.searchType).toBe('invoice');
    expect(clientControl?.value).toBe('');
    expect(invoiceControl?.hasError('required')).toBeTrue();
  });

  it('emits client search criteria and sets loading state', () => {
    spyOn(component.search, 'emit');

    component.searchForm.patchValue({
      searchType: 'client',
      clientId: 5
    });

    component.onSearch();

    expect(component.search.emit).toHaveBeenCalledWith(jasmine.objectContaining({
      searchType: 'client',
      clientId: 5,
      invoiceNumber: null
    } as any));
    expect(component.isLoading).toBeTrue();
    expect(component.hasSearched).toBeTrue();
  });

  it('emits invoice search criteria and clears clientId', () => {
    spyOn(component.search, 'emit');

    component.searchForm.patchValue({
      searchType: 'invoice',
      invoiceNumber: '1001'
    });

    component.onSearch();

    expect(component.search.emit).toHaveBeenCalledWith(jasmine.objectContaining({
      searchType: 'invoice',
      clientId: null,
      invoiceNumber: '1001'
    } as any));
  });
});
