
-- BLOG ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'BlogAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (10, 'BlogAdmin', 'Blog Administrator', 'blog',1,1,1, 1,1,getdate(), getdate())

IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'BlogPostAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (20, 'BlogPostAdmin', 'Blog Post Administrator', 'blogpost',1,1,1, 1,1,getdate(), getdate())

IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'BlogCommentAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (30, 'BlogCommentAdmin', 'Blog Comment Administrator', 'blogpostcomment',1,1,1, 1,1,getdate(), getdate())

-- CONTENT ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'ContentAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (40, 'ContentAdmin', 'Content Administrator', 'content',1,1,1, 1,1,getdate(), getdate())

IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'ContestAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (50, 'ContestAdmin', 'Contest Administrator', 'contest',1,1,1, 1,1,getdate(), getdate())

IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'ContestGroupAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (52, 'ContestGroupAdmin', 'Content Administrator', 'contestgroup',1,1,1, 1,1,getdate(), getdate())

-- CULTURE ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'CultureAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (60, 'CultureAdmin', 'Culture Administrator', 'culture',1,1,1, 1,1,getdate(), getdate())

-- FILE - RESOURCES ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'FileAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (70, 'FileAdmin', 'Resource Administrator', 'file',1,1,1, 1,1,getdate(), getdate())

IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'FileGroupAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (80, 'FileGroupAdmin', 'File/Resource Administrator', 'filegroup',1,1,1, 1,1,getdate(), getdate())

-- POLL ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'PollAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (90, 'PollAdmin', 'Poll Administrator', 'poll',1,1,1, 1,1,getdate(), getdate())

-- ROLE ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'RoleAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (100, 'RoleAdmin', 'Role Administrator', 'role',1,1,1, 1,1,getdate(), getdate())

-- SITE ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'SiteAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (110, 'SiteAdmin', 'Site Administrator', 'site',1,1,1, 0,0,getdate(), getdate())

-- USER ------------------------------------------------------- --
IF NOT EXISTS (SELECT * FROM awRole WHERE title = 'UserAdmin')
	INSERT INTO awRole (roleId,title,[description],module, canRead,canUpdateStatus,canUpdate,canAdd,canDelete,lastBuildDate,createDate)
	VALUES (120, 'UserAdmin', 'User Administrator', 'user',1,1,1, 1,1,getdate(), getdate())





