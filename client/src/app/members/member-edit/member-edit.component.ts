import { NgForOf } from '@angular/common';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})

export class MemberEditComponent implements OnInit {
  @ViewChild('formEdit') formEdit: NgForm | undefined
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.formEdit?.dirty) {
      $event.returnValue = true;
    }
  }//funzionalità del browser per non perdere 
  //le modifiche se si refresha la pagina

  //dentro viewchild metto il nome di cosa sto cercando
  member: Member | undefined;
  user: User | null = null;
  //all'inizio il nostro utente è null

  constructor(private accountService: AccountService, private membersService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    });
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    if (!this.user) return;
    this.membersService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    });
  }

  updateMember() {
    //console.log(this.member);
    this.membersService.updateMember(this.formEdit?.value).subscribe({
      next: _ => {
        this.toastr.success('Profilo Modificato');
        this.formEdit?.reset();
      }
    });
    
    //se quando modifico salvo e la textArea si svuota ho sbagliato
    // a scrivere il nome nell input  es scrivo
    //name="lookingFor" --> fiusto
    //name="lookingfor" --> sbagliatoe il campo non rimane pieno

  }
}
