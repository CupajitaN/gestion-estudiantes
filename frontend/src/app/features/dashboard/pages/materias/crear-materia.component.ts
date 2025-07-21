import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MateriaService } from '../../../../core/services/materia.service';

@Component({
  standalone: false,
  selector: 'app-crear-materia',
  templateUrl: './crear-materia.component.html',
})
export class CrearMateriaComponent implements OnInit {
  @Input() materia: any = null;
  @Output() materiaCreada = new EventEmitter<void>();
  @Output() cancelar = new EventEmitter<void>();

  materiaForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private materiaService: MateriaService
  ) {}

  ngOnInit(): void {
    this.materiaForm = this.fb.group({
      nombre: [this.materia?.nombre || '', Validators.required],
      descripcion: [this.materia?.descripcion || '', Validators.required],
    });
  }

  guardar(): void {
    const datos = this.materiaForm.value;

    if (this.materia) {
      this.materiaService.actualizar(this.materia.id, datos).subscribe(() => {
        this.materiaCreada.emit();
      });
    } else {
      this.materiaService.crear(datos).subscribe(() => {
        this.materiaForm.reset();
        this.materiaCreada.emit();
      });
    }
  }

  cancelarCreacion() {
    this.cancelar.emit();
  }
}
