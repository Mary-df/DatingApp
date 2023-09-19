import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit{

  @Input() member: Member | undefined;
  //@input poi lo passeremo al genitor 
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = true;
  baseUrl = environment.apiUrl;
  user: User | undefined;
  photos: any;

  constructor(private accountService: AccountService, private membersService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })
  }
  
  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo){
    this.membersService.setMainPhoto(photo.id).subscribe({
      next: () => {
        if(this.user && this.member) {
          this.user.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
          this.member.photoUrl = photo.url;
          this.member.photos.forEach(p => {
            if(p.isMain) p.isMain = false;
            if(p.id == photo.id) p.isMain = true;
          })
        }
      }
    });
  }
  
  /* deletePhoto(photoId: number){
    this.membersService.deletePhoto(photoId).subscribe({
      next: _ => {
        if(this.member) {
          this.member.photos = this.photos.filter(x => x.id !== photoId);
        }
      }
    })
  } */

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Beare ' + this.user?.token,
      isHTML5: true, //specifichiamo che si tratta di html
      allowedFileType: ['image'], //si specifica i tipi di file consentiti con image specifico jpeg png etc
      removeAfterUpload: true,//rimuovi dopo oil caricamento
      autoUpload: false,//gli utenti dovranno fare clic sul pulsante per caricare la foto
      maxFileSize: 10 * 1024 * 1024,//impostiamo ma dimensione massima del file a 10 mega perchè è la misuara massima consentita dal cloudinary
      //disableMultipart: true,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
      //il punto del file con le credenziali è false
      //se non lo facciamo dovremmo modificare la configurazione del cors
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      //cosa deve fare dopo essere caricato con successo
      if (response) {
        const photo = JSON.parse(response);
        this.member?.photos.push(photo);
        if(photo.isMain && this.user && this.member){
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
        }
      }
    }

  }

}
