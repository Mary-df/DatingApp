import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit{
  //members: Member[]= [];
  //members lo facciamo diventare observable
  members$: Observable<Member[]> | undefined;
  

  constructor(private membersService: MembersService) {}
  
  ngOnInit(): void {
    //quindi dopo l'observable questo diventa
    this.members$ = this.membersService.getMembers();
  }

  //dopo aver messo l'observable quello che segue si può ca
  /* loadMembers(){
    this.membersService.getMembers().subscribe({
      next: members => this.members = members
    });
  } */

}
