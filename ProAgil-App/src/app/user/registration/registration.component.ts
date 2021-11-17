import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { AuthService } from 'src/app/_services/auth.service';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registerForm: FormGroup | any;
  user: User | any;

  constructor(private authService: AuthService
    , public router: Router
    , public fb: FormBuilder
    , private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.validation();
  }
  validation() {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(150)]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required, Validators.maxLength(40), Validators.minLength(5)]],
      passwords: this.fb.group({
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required]
      }, { validator: this.compararSenhas })
    });
  }

  compararSenhas(fb: FormGroup) {
    const confirmSenhaCtrl = fb.get('confirmPassword');
    if (confirmSenhaCtrl?.errors == null || 'mismatch' in confirmSenhaCtrl.errors) {
      if (fb.get('password')?.value !== confirmSenhaCtrl?.value) {
        confirmSenhaCtrl?.setErrors({ mismatch: true });
      } else {
        confirmSenhaCtrl?.setErrors(null);
      }
    }
  }

  cadastrarUsuario() {

    if (this.registerForm.valid) {
      // mapeia o campo password dentro do objeto User
      // passa os valores do formulario para o objeto User
      this.user = Object.assign(
        { password: this.registerForm.get('passwords.password').value },
        this.registerForm.value);

      this.authService.register(this.user).subscribe(
        // Sucesso
        () => {
          this.router.navigate(['/user/login']); // direciona para pagina de login
          this.toastr.success('Cadastro Realizado');
        },
        //Erro
        error => {
          const erro = error.error;
          // percorre o erro para mostrar ao usuário
          erro.forEach((element : any)  => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('Cadastro Duplicado!');
                break;
              default:
                this.toastr.error(`Erro no Cadastro! Code: (${element.code})`);
                break;
            }
          });
        }

      );
    }
  }

}
